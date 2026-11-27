using DataBase.Model.EntitiesInventory;
using DataBase.Model.SourceGenerator;

using System.Text.Json;

namespace DataBase.Data.SqlQuery;

public static class DayQueryExtensions
{
    private const long DotNetTicksToUnixEpoch = 621355968000000000L;
    private const long TicksPerDay = 864000000000L;
    private const long MondayEpochOffsetTicks = 621353376000000000L;

    /// <summary>
    /// Zwraca skrypt SQL tworzący indeksy pokrywające (Covering Indexes) dopasowane do rzeczywistego schematu tabel Day, Product, ProductName i Cake.
    /// Uruchom jednorazowo przy inicjalizacji lub migracji bazy danych.
    /// </summary>
    public static string GetRecommendedIndexesSql() => $@"
        CREATE INDEX IF NOT EXISTS idx_{nameof(Day)}_{nameof(Day.SelectedDateTicks)}_{nameof(Day.SelectedRoute)}
            ON {nameof(Day)}(
                {nameof(Day.SelectedDateTicks)},
                {nameof(Day.SelectedRoute)},
                {nameof(Day.UserCreatedId)},
                {nameof(Day.TotalPrice)},
                {nameof(Day.TotalPriceProducts)},
                {nameof(Day.TotalPriceCake)},
                {nameof(Day.TotalPriceCorrect)},
                {nameof(Day.TotalPriceAfterCorrect)},
                {nameof(Day.TotalPriceMoney)},
                {nameof(Day.TotalPriceDifference)}
            );

        CREATE INDEX IF NOT EXISTS idx_{nameof(Product)}_{nameof(Product.DayId)}_Covering
            ON {nameof(Product)}(
                {nameof(Product.DayId)} COLLATE NOCASE,
                {nameof(Product.IsDelete)},
                {nameof(Product.ProductNameId)},
                {nameof(Product.Number)},
                {nameof(Product.NumberEdit)},
                {nameof(Product.NumberReturn)},
                {nameof(Product.PriceTotal)},
                {nameof(Product.PriceTotalCorrect)},
                {nameof(Product.PriceTotalAfterCorrect)}
            );

        CREATE INDEX IF NOT EXISTS idx_{nameof(ProductName)}_{nameof(ProductName.Id)}_NoCase
            ON {nameof(ProductName)}(
                {nameof(ProductName.Id)} COLLATE NOCASE,
                {nameof(ProductName.Arrangement)},
                {nameof(ProductName.Name)},
                {nameof(ProductName.Description)},
                {nameof(ProductName.Img)},
                {nameof(ProductName.IsVisible)},
                {nameof(ProductName.IsDelete)}
            );

        CREATE INDEX IF NOT EXISTS idx_{nameof(Cake)}_{nameof(Cake.DayId)}_NoCase
            ON {nameof(Cake)}({nameof(Cake.DayId)} COLLATE NOCASE, {nameof(Cake.IsDelete)});

        ANALYZE;";

    /// <summary>
    /// Rzutuje wynik agregacji na liczbę całkowitą (INTEGER) zaokrąglając do najbliższej wartości,
    /// aby zapobiec zwracaniu typu zmiennoprzecinkowego (REAL) przez funkcję AVG() w SQLite.
    /// </summary>
    private static string AsInt(string sqlExpression)
        => $"CAST(ROUND(COALESCE({sqlExpression}, 0), 0) AS INTEGER)";

    private static string DateFromTicks(string ticksColumn)
        => $"datetime(({ticksColumn} - {DotNetTicksToUnixEpoch}) / 10000000, 'unixepoch')";

    private static string FastPeriodKey(string ticksColumn, DayCalculationType calculation)
        => calculation switch
        {
            DayCalculationType.SumWeek or DayCalculationType.AverageWeek or DayCalculationType.MedianWeek
                => $"(({ticksColumn} - {MondayEpochOffsetTicks}) / {TicksPerDay * 7})",

            DayCalculationType.SumMonth or DayCalculationType.AverageMonth or DayCalculationType.MedianMonth
                => $"(CAST(strftime('%Y', {DateFromTicks(ticksColumn)}) AS INTEGER) * 12 + CAST(strftime('%m', {DateFromTicks(ticksColumn)}) AS INTEGER))",

            DayCalculationType.SumYear or DayCalculationType.AverageYear or DayCalculationType.MedianYear
                => $"CAST(strftime('%Y', {DateFromTicks(ticksColumn)}) AS INTEGER)",

            DayCalculationType.SumDayOfWeek or DayCalculationType.AverageDayOfWeek or DayCalculationType.MedianDayOfWeek
                => $"(((({ticksColumn} - {MondayEpochOffsetTicks}) / {TicksPerDay}) % 7) + 1)",

            _ => "0"
        };

    private static string FormatSelectedDateString(string minTicksCol, string maxTicksCol, DayCalculationType calculation)
    {
        string minDt = DateFromTicks(minTicksCol);
        string maxDt = DateFromTicks(maxTicksCol);
        string weekStart = $"date({minDt}, '-6 days', 'weekday 1')";
        string weekEnd = $"date({weekStart}, '+6 days')";

        return calculation switch
        {
            DayCalculationType.SumWeek or DayCalculationType.AverageWeek or DayCalculationType.MedianWeek
                => $"(CAST(CAST(strftime('%d', {weekStart}) AS INTEGER) AS TEXT) || '.' || CAST(CAST(strftime('%m', {weekStart}) AS INTEGER) AS TEXT) || '-' || CAST(CAST(strftime('%d', {weekEnd}) AS INTEGER) AS TEXT) || '.' || CAST(CAST(strftime('%m', {weekEnd}) AS INTEGER) AS TEXT) || '.' || strftime('%Y', {weekEnd}))",

            DayCalculationType.SumMonth or DayCalculationType.AverageMonth or DayCalculationType.MedianMonth
                => $"(CAST(CAST(strftime('%m', {minDt}) AS INTEGER) AS TEXT) || '.' || strftime('%Y', {minDt}))",

            DayCalculationType.SumYear or DayCalculationType.AverageYear or DayCalculationType.MedianYear
                => $"strftime('%Y', {minDt})",

            DayCalculationType.SumDayOfWeek or DayCalculationType.AverageDayOfWeek or DayCalculationType.MedianDayOfWeek
                => $@"CASE (((({minTicksCol} - {MondayEpochOffsetTicks}) / {TicksPerDay}) % 7) + 1)
                        WHEN 1 THEN 'Poniedziałek'
                        WHEN 2 THEN 'Wtorek'
                        WHEN 3 THEN 'Środa'
                        WHEN 4 THEN 'Czwartek'
                        WHEN 5 THEN 'Piątek'
                        WHEN 6 THEN 'Sobota'
                        WHEN 7 THEN 'Niedziela'
                        ELSE ''
                      END",

            _ => $"(CAST(CAST(strftime('%d', {minDt}) AS INTEGER) AS TEXT) || '.' || CAST(CAST(strftime('%m', {minDt}) AS INTEGER) AS TEXT) || '-' || CAST(CAST(strftime('%d', {maxDt}) AS INTEGER) AS TEXT) || '.' || CAST(CAST(strftime('%m', {maxDt}) AS INTEGER) AS TEXT) || '.' || strftime('%Y', {maxDt}))"
        };
    }

    private static string ProductNameJsonObject(string alias) => $@"
        CASE WHEN {alias}.{nameof(ProductName.Id)} IS NULL THEN NULL
        ELSE json_object(
            '{nameof(ProductName.Id)}', {alias}.{nameof(ProductName.Id)},
            '{nameof(ProductName.Arrangement)}', {alias}.{nameof(ProductName.Arrangement)},
            '{nameof(ProductName.Name)}', {alias}.{nameof(ProductName.Name)},
            '{nameof(ProductName.Description)}', {alias}.{nameof(ProductName.Description)},
            '{nameof(ProductName.Img)}', {alias}.{nameof(ProductName.Img)},
            '{nameof(ProductName.IsVisible)}', {alias}.{nameof(ProductName.IsVisible)},
            '{nameof(ProductName.CreatedTicks)}', {alias}.{nameof(ProductName.CreatedTicks)},
            '{nameof(ProductName.UpdatedTicks)}', {alias}.{nameof(ProductName.UpdatedTicks)},
            '{nameof(ProductName.IsDelete)}', {alias}.{nameof(ProductName.IsDelete)},
            '{nameof(ProductName.UserCreatedId)}', {alias}.{nameof(ProductName.UserCreatedId)},
            '{nameof(ProductName.UserUpdatedId)}', {alias}.{nameof(ProductName.UserUpdatedId)}
        ) END";

    public static string GetDayWithProductsAndAggregatesQuery(DayCalculationType calculation, long fromTicks, long toTicks, params Guid[] routeIds)
    {
        string where = $"WHERE d.{nameof(Day.SelectedDateTicks)} BETWEEN {fromTicks} AND {toTicks}";

        string jsonRouteIds = JsonSerializer.Serialize(routeIds, SzarotkaJsonSerializerContext.Default.IListGuid);
        if (routeIds.Length > 0)
        {
            where += $" AND d.{nameof(Day.SelectedRoute)} IN (SELECT value FROM json_each(@{nameof(jsonRouteIds)}))";
        }

        string periodKey = FastPeriodKey($"d.{nameof(Day.SelectedDateTicks)}", calculation);
        string selectedDateStringExpr = FormatSelectedDateString("d.MinTicks", "d.MaxTicks", calculation);

        string baseDayCte = $@"
            base_day AS MATERIALIZED (
                SELECT
                    d.{nameof(Day.Id)} AS {nameof(Day.Id)},
                    d.{nameof(Day.SelectedRoute)} AS {nameof(Day.SelectedRoute)},
                    d.{nameof(Day.UserCreatedId)} AS {nameof(Day.UserCreatedId)},
                    {periodKey} AS PeriodKey,
                    d.{nameof(Day.SelectedDateTicks)} AS {nameof(Day.SelectedDateTicks)},
                    COALESCE(d.{nameof(Day.TotalPrice)}, 0) AS {nameof(Day.TotalPrice)},
                    COALESCE(d.{nameof(Day.TotalPriceProducts)}, 0) AS {nameof(Day.TotalPriceProducts)},
                    COALESCE(d.{nameof(Day.TotalPriceCake)}, 0) AS {nameof(Day.TotalPriceCake)},
                    COALESCE(d.{nameof(Day.TotalPriceCorrect)}, 0) AS {nameof(Day.TotalPriceCorrect)},
                    COALESCE(d.{nameof(Day.TotalPriceAfterCorrect)}, 0) AS {nameof(Day.TotalPriceAfterCorrect)},
                    COALESCE(d.{nameof(Day.TotalPriceMoney)}, 0) AS {nameof(Day.TotalPriceMoney)},
                    COALESCE(d.{nameof(Day.TotalPriceDifference)}, 0) AS {nameof(Day.TotalPriceDifference)}
                FROM {nameof(Day)} d
                {where}
            )";

        string buildStandardAggregateQuery(string sqlFunc) => $@"
            WITH {baseDayCte},
            agg_day AS (
                SELECT
                    {nameof(Day.SelectedRoute)},
                    PeriodKey,
                    MIN({nameof(Day.SelectedDateTicks)}) AS MinTicks,
                    MAX({nameof(Day.SelectedDateTicks)}) AS MaxTicks,
                    MIN({nameof(Day.UserCreatedId)}) AS {nameof(Day.UserCreatedId)},
                    {AsInt($"{sqlFunc}({nameof(Day.TotalPrice)})")} AS {nameof(Day.TotalPrice)},
                    {AsInt($"{sqlFunc}({nameof(Day.TotalPriceProducts)})")} AS {nameof(Day.TotalPriceProducts)},
                    {AsInt($"{sqlFunc}({nameof(Day.TotalPriceCake)})")} AS {nameof(Day.TotalPriceCake)},
                    {AsInt($"{sqlFunc}({nameof(Day.TotalPriceCorrect)})")} AS {nameof(Day.TotalPriceCorrect)},
                    {AsInt($"{sqlFunc}({nameof(Day.TotalPriceAfterCorrect)})")} AS {nameof(Day.TotalPriceAfterCorrect)},
                    {AsInt($"{sqlFunc}({nameof(Day.TotalPriceMoney)})")} AS {nameof(Day.TotalPriceMoney)},
                    {AsInt($"{sqlFunc}({nameof(Day.TotalPriceDifference)})")} AS {nameof(Day.TotalPriceDifference)}
                FROM base_day
                GROUP BY {nameof(Day.SelectedRoute)}, PeriodKey
            ),
            agg_products_raw AS (
                SELECT
                    bd.{nameof(Day.SelectedRoute)},
                    bd.PeriodKey,
                    p.{nameof(Product.ProductNameId)} AS {nameof(Product.ProductNameId)},
                    {AsInt($"{sqlFunc}(COALESCE(p.{nameof(Product.Number)}, 0))")} AS {nameof(Product.Number)},
                    {AsInt($"{sqlFunc}(COALESCE(p.{nameof(Product.NumberEdit)}, 0))")} AS {nameof(Product.NumberEdit)},
                    {AsInt($"{sqlFunc}(COALESCE(p.{nameof(Product.NumberReturn)}, 0))")} AS {nameof(Product.NumberReturn)},
                    {AsInt($"{sqlFunc}(COALESCE(p.{nameof(Product.PriceTotal)}, 0))")} AS {nameof(Product.PriceTotal)},
                    {AsInt($"{sqlFunc}(COALESCE(p.{nameof(Product.PriceTotalCorrect)}, 0))")} AS {nameof(Product.PriceTotalCorrect)},
                    {AsInt($"{sqlFunc}(COALESCE(p.{nameof(Product.PriceTotalAfterCorrect)}, 0))")} AS {nameof(Product.PriceTotalAfterCorrect)}
                FROM {nameof(Product)} p
                INNER JOIN base_day bd ON bd.{nameof(Day.Id)} = p.{nameof(Product.DayId)} COLLATE NOCASE
                WHERE (p.{nameof(Product.IsDelete)} = 0 OR p.{nameof(Product.IsDelete)} IS NULL)
                GROUP BY bd.{nameof(Day.SelectedRoute)}, bd.PeriodKey, p.{nameof(Product.ProductNameId)}
            ),
            agg_products_sorted AS (
                SELECT
                    apr.{nameof(Day.SelectedRoute)},
                    apr.PeriodKey,
                    json_object(
                        '{nameof(Product.ProductNameId)}', apr.{nameof(Product.ProductNameId)},
                        '{nameof(Product.Number)}', apr.{nameof(Product.Number)},
                        '{nameof(Product.NumberEdit)}', apr.{nameof(Product.NumberEdit)},
                        '{nameof(Product.NumberReturn)}', apr.{nameof(Product.NumberReturn)},
                        '{nameof(Product.PriceTotal)}', apr.{nameof(Product.PriceTotal)},
                        '{nameof(Product.PriceTotalCorrect)}', apr.{nameof(Product.PriceTotalCorrect)},
                        '{nameof(Product.PriceTotalAfterCorrect)}', apr.{nameof(Product.PriceTotalAfterCorrect)},
                        '{nameof(ProductName)}', {ProductNameJsonObject("pn")}
                    ) AS ProductJson
                FROM agg_products_raw apr
                LEFT JOIN {nameof(ProductName)} pn ON pn.{nameof(ProductName.Id)} = apr.{nameof(Product.ProductNameId)} COLLATE NOCASE
                ORDER BY apr.{nameof(Day.SelectedRoute)}, apr.PeriodKey, COALESCE(pn.{nameof(ProductName.Arrangement)}, 2147483647), pn.{nameof(ProductName.Name)}
            ),
            agg_products AS (
                SELECT
                    {nameof(Day.SelectedRoute)},
                    PeriodKey,
                    json_group_array(json(ProductJson)) AS JsonProducts
                FROM agg_products_sorted
                GROUP BY {nameof(Day.SelectedRoute)}, PeriodKey
            )
            SELECT
                {selectedDateStringExpr} AS {nameof(Day.SelectedDateString)},
                d.MinTicks AS {nameof(Day.SelectedDateTicks)},
                d.{nameof(Day.SelectedRoute)},
                d.{nameof(Day.UserCreatedId)},
                d.{nameof(Day.TotalPrice)},
                d.{nameof(Day.TotalPriceProducts)},
                d.{nameof(Day.TotalPriceCake)},
                d.{nameof(Day.TotalPriceCorrect)},
                d.{nameof(Day.TotalPriceAfterCorrect)},
                d.{nameof(Day.TotalPriceMoney)},
                d.{nameof(Day.TotalPriceDifference)},
                COALESCE(p.JsonProducts, '[]') AS JsonProducts
            FROM agg_day d
            LEFT JOIN agg_products p
                ON p.{nameof(Day.SelectedRoute)} IS d.{nameof(Day.SelectedRoute)}
               AND p.PeriodKey = d.PeriodKey
            ORDER BY d.MinTicks, d.{nameof(Day.SelectedRoute)};";

        string buildMedianAggregateQuery() => $@"
            WITH {baseDayCte},
            day_prepared AS MATERIALIZED (
                SELECT
                    {nameof(Day.Id)},
                    {nameof(Day.SelectedRoute)},
                    {nameof(Day.UserCreatedId)},
                    PeriodKey,
                    {nameof(Day.SelectedDateTicks)},
                    DENSE_RANK() OVER (ORDER BY {nameof(Day.SelectedRoute)}, PeriodKey) AS GrpId,
                    COUNT(*) OVER (PARTITION BY {nameof(Day.SelectedRoute)}, PeriodKey) AS Cnt,
                    {nameof(Day.TotalPrice)},
                    {nameof(Day.TotalPriceProducts)},
                    {nameof(Day.TotalPriceCake)},
                    {nameof(Day.TotalPriceCorrect)},
                    {nameof(Day.TotalPriceAfterCorrect)},
                    {nameof(Day.TotalPriceMoney)},
                    {nameof(Day.TotalPriceDifference)}
                FROM base_day
            ),
            day_groups AS (
                SELECT
                    GrpId,
                    MIN({nameof(Day.SelectedRoute)}) AS {nameof(Day.SelectedRoute)},
                    MIN({nameof(Day.UserCreatedId)}) AS {nameof(Day.UserCreatedId)},
                    MIN({nameof(Day.SelectedDateTicks)}) AS MinTicks,
                    MAX({nameof(Day.SelectedDateTicks)}) AS MaxTicks
                FROM day_prepared
                GROUP BY GrpId
            ),
            day_ranked AS (
                SELECT
                    GrpId,
                    Cnt,
                    {nameof(Day.TotalPrice)},
                    {nameof(Day.TotalPriceProducts)},
                    {nameof(Day.TotalPriceCake)},
                    {nameof(Day.TotalPriceCorrect)},
                    {nameof(Day.TotalPriceAfterCorrect)},
                    {nameof(Day.TotalPriceMoney)},
                    {nameof(Day.TotalPriceDifference)},
                    ROW_NUMBER() OVER (PARTITION BY GrpId ORDER BY {nameof(Day.TotalPrice)}) AS rn1,
                    ROW_NUMBER() OVER (PARTITION BY GrpId ORDER BY {nameof(Day.TotalPriceProducts)}) AS rn2,
                    ROW_NUMBER() OVER (PARTITION BY GrpId ORDER BY {nameof(Day.TotalPriceCake)}) AS rn3,
                    ROW_NUMBER() OVER (PARTITION BY GrpId ORDER BY {nameof(Day.TotalPriceCorrect)}) AS rn4,
                    ROW_NUMBER() OVER (PARTITION BY GrpId ORDER BY {nameof(Day.TotalPriceAfterCorrect)}) AS rn5,
                    ROW_NUMBER() OVER (PARTITION BY GrpId ORDER BY {nameof(Day.TotalPriceMoney)}) AS rn6,
                    ROW_NUMBER() OVER (PARTITION BY GrpId ORDER BY {nameof(Day.TotalPriceDifference)}) AS rn7
                FROM day_prepared
            ),
            agg_day AS (
                SELECT
                    GrpId,
                    {AsInt($"AVG(CASE WHEN rn1 IN ((Cnt + 1) / 2, (Cnt + 2) / 2) THEN {nameof(Day.TotalPrice)} END)")} AS {nameof(Day.TotalPrice)},
                    {AsInt($"AVG(CASE WHEN rn2 IN ((Cnt + 1) / 2, (Cnt + 2) / 2) THEN {nameof(Day.TotalPriceProducts)} END)")} AS {nameof(Day.TotalPriceProducts)},
                    {AsInt($"AVG(CASE WHEN rn3 IN ((Cnt + 1) / 2, (Cnt + 2) / 2) THEN {nameof(Day.TotalPriceCake)} END)")} AS {nameof(Day.TotalPriceCake)},
                    {AsInt($"AVG(CASE WHEN rn4 IN ((Cnt + 1) / 2, (Cnt + 2) / 2) THEN {nameof(Day.TotalPriceCorrect)} END)")} AS {nameof(Day.TotalPriceCorrect)},
                    {AsInt($"AVG(CASE WHEN rn5 IN ((Cnt + 1) / 2, (Cnt + 2) / 2) THEN {nameof(Day.TotalPriceAfterCorrect)} END)")} AS {nameof(Day.TotalPriceAfterCorrect)},
                    {AsInt($"AVG(CASE WHEN rn6 IN ((Cnt + 1) / 2, (Cnt + 2) / 2) THEN {nameof(Day.TotalPriceMoney)} END)")} AS {nameof(Day.TotalPriceMoney)},
                    {AsInt($"AVG(CASE WHEN rn7 IN ((Cnt + 1) / 2, (Cnt + 2) / 2) THEN {nameof(Day.TotalPriceDifference)} END)")} AS {nameof(Day.TotalPriceDifference)}
                FROM day_ranked
                GROUP BY GrpId
            ),
            products_prepared AS MATERIALIZED (
                SELECT
                    dp.GrpId,
                    p.{nameof(Product.ProductNameId)} AS {nameof(Product.ProductNameId)},
                    COUNT(*) OVER (PARTITION BY dp.GrpId, p.{nameof(Product.ProductNameId)}) AS Cnt,
                    COALESCE(p.{nameof(Product.Number)}, 0) AS {nameof(Product.Number)},
                    COALESCE(p.{nameof(Product.NumberEdit)}, 0) AS {nameof(Product.NumberEdit)},
                    COALESCE(p.{nameof(Product.NumberReturn)}, 0) AS {nameof(Product.NumberReturn)},
                    COALESCE(p.{nameof(Product.PriceTotal)}, 0) AS {nameof(Product.PriceTotal)},
                    COALESCE(p.{nameof(Product.PriceTotalCorrect)}, 0) AS {nameof(Product.PriceTotalCorrect)},
                    COALESCE(p.{nameof(Product.PriceTotalAfterCorrect)}, 0) AS {nameof(Product.PriceTotalAfterCorrect)}
                FROM {nameof(Product)} p
                INNER JOIN day_prepared dp ON dp.{nameof(Day.Id)} = p.{nameof(Product.DayId)} COLLATE NOCASE
                WHERE (p.{nameof(Product.IsDelete)} = 0 OR p.{nameof(Product.IsDelete)} IS NULL)
            ),
            products_ranked AS (
                SELECT
                    GrpId,
                    {nameof(Product.ProductNameId)},
                    Cnt,
                    {nameof(Product.Number)},
                    {nameof(Product.NumberEdit)},
                    {nameof(Product.NumberReturn)},
                    {nameof(Product.PriceTotal)},
                    {nameof(Product.PriceTotalCorrect)},
                    {nameof(Product.PriceTotalAfterCorrect)},
                    ROW_NUMBER() OVER (PARTITION BY GrpId, {nameof(Product.ProductNameId)} ORDER BY {nameof(Product.Number)}) AS rn1,
                    ROW_NUMBER() OVER (PARTITION BY GrpId, {nameof(Product.ProductNameId)} ORDER BY {nameof(Product.NumberEdit)}) AS rn2,
                    ROW_NUMBER() OVER (PARTITION BY GrpId, {nameof(Product.ProductNameId)} ORDER BY {nameof(Product.NumberReturn)}) AS rn3,
                    ROW_NUMBER() OVER (PARTITION BY GrpId, {nameof(Product.ProductNameId)} ORDER BY {nameof(Product.PriceTotal)}) AS rn4,
                    ROW_NUMBER() OVER (PARTITION BY GrpId, {nameof(Product.ProductNameId)} ORDER BY {nameof(Product.PriceTotalCorrect)}) AS rn5,
                    ROW_NUMBER() OVER (PARTITION BY GrpId, {nameof(Product.ProductNameId)} ORDER BY {nameof(Product.PriceTotalAfterCorrect)}) AS rn6
                FROM products_prepared
            ),
            agg_products_raw AS (
                SELECT
                    GrpId,
                    {nameof(Product.ProductNameId)},
                    {AsInt($"AVG(CASE WHEN rn1 IN ((Cnt + 1) / 2, (Cnt + 2) / 2) THEN {nameof(Product.Number)} END)")} AS {nameof(Product.Number)},
                    {AsInt($"AVG(CASE WHEN rn2 IN ((Cnt + 1) / 2, (Cnt + 2) / 2) THEN {nameof(Product.NumberEdit)} END)")} AS {nameof(Product.NumberEdit)},
                    {AsInt($"AVG(CASE WHEN rn3 IN ((Cnt + 1) / 2, (Cnt + 2) / 2) THEN {nameof(Product.NumberReturn)} END)")} AS {nameof(Product.NumberReturn)},
                    {AsInt($"AVG(CASE WHEN rn4 IN ((Cnt + 1) / 2, (Cnt + 2) / 2) THEN {nameof(Product.PriceTotal)} END)")} AS {nameof(Product.PriceTotal)},
                    {AsInt($"AVG(CASE WHEN rn5 IN ((Cnt + 1) / 2, (Cnt + 2) / 2) THEN {nameof(Product.PriceTotalCorrect)} END)")} AS {nameof(Product.PriceTotalCorrect)},
                    {AsInt($"AVG(CASE WHEN rn6 IN ((Cnt + 1) / 2, (Cnt + 2) / 2) THEN {nameof(Product.PriceTotalAfterCorrect)} END)")} AS {nameof(Product.PriceTotalAfterCorrect)}
                FROM products_ranked
                GROUP BY GrpId, {nameof(Product.ProductNameId)}
            ),
            agg_products_sorted AS (
                SELECT
                    apr.GrpId,
                    json_object(
                        '{nameof(Product.ProductNameId)}', apr.{nameof(Product.ProductNameId)},
                        '{nameof(Product.Number)}', apr.{nameof(Product.Number)},
                        '{nameof(Product.NumberEdit)}', apr.{nameof(Product.NumberEdit)},
                        '{nameof(Product.NumberReturn)}', apr.{nameof(Product.NumberReturn)},
                        '{nameof(Product.PriceTotal)}', apr.{nameof(Product.PriceTotal)},
                        '{nameof(Product.PriceTotalCorrect)}', apr.{nameof(Product.PriceTotalCorrect)},
                        '{nameof(Product.PriceTotalAfterCorrect)}', apr.{nameof(Product.PriceTotalAfterCorrect)},
                        '{nameof(ProductName)}', {ProductNameJsonObject("pn")}
                    ) AS ProductJson
                FROM agg_products_raw apr
                LEFT JOIN {nameof(ProductName)} pn ON pn.{nameof(ProductName.Id)} = apr.{nameof(Product.ProductNameId)} COLLATE NOCASE
                ORDER BY apr.GrpId, COALESCE(pn.{nameof(ProductName.Arrangement)}, 2147483647), pn.{nameof(ProductName.Name)}
            ),
            agg_products AS (
                SELECT
                    GrpId,
                    json_group_array(json(ProductJson)) AS JsonProducts
                FROM agg_products_sorted
                GROUP BY GrpId
            )
            SELECT
                {selectedDateStringExpr} AS {nameof(Day.SelectedDateString)},
                d.MinTicks AS {nameof(Day.SelectedDateTicks)},
                d.{nameof(Day.SelectedRoute)},
                d.{nameof(Day.UserCreatedId)},
                ad.{nameof(Day.TotalPrice)},
                ad.{nameof(Day.TotalPriceProducts)},
                ad.{nameof(Day.TotalPriceCake)},
                ad.{nameof(Day.TotalPriceCorrect)},
                ad.{nameof(Day.TotalPriceAfterCorrect)},
                ad.{nameof(Day.TotalPriceMoney)},
                ad.{nameof(Day.TotalPriceDifference)},
                COALESCE(p.JsonProducts, '[]') AS JsonProducts
            FROM agg_day ad
            INNER JOIN day_groups d ON d.GrpId = ad.GrpId
            LEFT JOIN agg_products p ON p.GrpId = ad.GrpId
            ORDER BY d.MinTicks, d.{nameof(Day.SelectedRoute)};";

        string buildRawQuery() => $@"
            SELECT
                d.*,
                COALESCE((
                    SELECT json_group_array(json(ProductJson))
                    FROM (
                        SELECT
                            json_object(
                                '{nameof(Product.Id)}', p.{nameof(Product.Id)},
                                '{nameof(Product.DayId)}', p.{nameof(Product.DayId)},
                                '{nameof(Product.ProductNameId)}', p.{nameof(Product.ProductNameId)},
                                '{nameof(Product.ProductPriceId)}', p.{nameof(Product.ProductPriceId)},
                                '{nameof(Product.Description)}', p.{nameof(Product.Description)},
                                '{nameof(Product.PriceTotal)}', p.{nameof(Product.PriceTotal)},
                                '{nameof(Product.PriceTotalCorrect)}', p.{nameof(Product.PriceTotalCorrect)},
                                '{nameof(Product.PriceTotalAfterCorrect)}', p.{nameof(Product.PriceTotalAfterCorrect)},
                                '{nameof(Product.Number)}', p.{nameof(Product.Number)},
                                '{nameof(Product.NumberEdit)}', p.{nameof(Product.NumberEdit)},
                                '{nameof(Product.NumberReturn)}', p.{nameof(Product.NumberReturn)},
                                '{nameof(Product.CreatedTicks)}', p.{nameof(Product.CreatedTicks)},
                                '{nameof(Product.UpdatedTicks)}', p.{nameof(Product.UpdatedTicks)},
                                '{nameof(Product.UserCreatedId)}', p.{nameof(Product.UserCreatedId)},
                                '{nameof(Product.IsDelete)}', p.{nameof(Product.IsDelete)},
                                '{nameof(Product.UserUpdatedId)}', p.{nameof(Product.UserUpdatedId)},
                                '{nameof(ProductName)}', {ProductNameJsonObject("pn")}
                            ) AS ProductJson
                        FROM {nameof(Product)} p
                        LEFT JOIN {nameof(ProductName)} pn ON pn.{nameof(ProductName.Id)} = p.{nameof(Product.ProductNameId)} COLLATE NOCASE
                        WHERE p.{nameof(Product.DayId)} = d.{nameof(Day.Id)} COLLATE NOCASE
                          AND (p.{nameof(Product.IsDelete)} = 0 OR p.{nameof(Product.IsDelete)} IS NULL)
                        ORDER BY COALESCE(pn.{nameof(ProductName.Arrangement)}, 2147483647), pn.{nameof(ProductName.Name)}
                    )
                ), '[]') AS JsonProducts,
                COALESCE((
                    SELECT json_group_array(
                        json_object(
                            '{nameof(Cake.Id)}', c.{nameof(Cake.Id)},
                            '{nameof(Cake.DayId)}', c.{nameof(Cake.DayId)},
                            '{nameof(Cake.IsSell)}', c.{nameof(Cake.IsSell)},
                            '{nameof(Cake.Price)}', c.{nameof(Cake.Price)},
                            '{nameof(Cake.CreatedTicks)}', c.{nameof(Cake.CreatedTicks)},
                            '{nameof(Cake.UpdatedTicks)}', c.{nameof(Cake.UpdatedTicks)},
                            '{nameof(Cake.UserCreatedId)}', c.{nameof(Cake.UserCreatedId)},
                            '{nameof(Cake.IsDelete)}', c.{nameof(Cake.IsDelete)},
                            '{nameof(Cake.UserUpdatedId)}', c.{nameof(Cake.UserUpdatedId)}
                        )
                    )
                    FROM {nameof(Cake)} c
                    WHERE c.{nameof(Cake.DayId)} = d.{nameof(Day.Id)} COLLATE NOCASE
                      AND (c.{nameof(Cake.IsDelete)} = 0 OR c.{nameof(Cake.IsDelete)} IS NULL)
                ), '[]') AS JsonCakes
            FROM {nameof(Day)} d
            {where}
            ORDER BY d.{nameof(Day.SelectedDateTicks)};";

        return calculation switch
        {
            DayCalculationType.None => buildRawQuery(),

            DayCalculationType.SumWeek or DayCalculationType.SumMonth or DayCalculationType.SumYear or DayCalculationType.SumDayOfWeek or DayCalculationType.SumAll
                => buildStandardAggregateQuery("SUM"),

            DayCalculationType.AverageWeek or DayCalculationType.AverageMonth or DayCalculationType.AverageYear or DayCalculationType.AverageDayOfWeek or DayCalculationType.AverageAll
                => buildStandardAggregateQuery("AVG"),

            DayCalculationType.MedianWeek or DayCalculationType.MedianMonth or DayCalculationType.MedianYear or DayCalculationType.MedianDayOfWeek or DayCalculationType.MedianAll
                => buildMedianAggregateQuery(),

            _ => buildRawQuery()
        };
    }
}
