using DataBase.Model.EntitiesInventory;

namespace DataBase.Data.SqlQuery;

public static class DayQueryExtensions
{
    private const long DotNetTicksToUnixEpoch = 621355968000000000L;

    private static string DateFromTicks(string ticksColumn)
        => $"datetime(({ticksColumn} - {DotNetTicksToUnixEpoch}) / 10000000, 'unixepoch')";

    private static string DayOfWeekKey(string ticksColumn)
        => $"CAST(((strftime('%w', {DateFromTicks(ticksColumn)}) + 6) % 7) + 1 AS INTEGER)";

    private static string WeekStartKey(string ticksColumn)
        => $"date({DateFromTicks(ticksColumn)}, '-6 days', 'weekday 1')";

    private static string MonthKey(string ticksColumn)
        => $"strftime('%Y-%m', {DateFromTicks(ticksColumn)})";

    private static string YearKey(string ticksColumn)
        => $"strftime('%Y', {DateFromTicks(ticksColumn)})";


    public static string GetDayWithProductsAndAggregatesQuery(DayCalculationType calculation, long fromTicks, long toTicks)
    {
        string where = $"WHERE d.{nameof(Day.SelectedDateTicks)} BETWEEN {fromTicks} AND {toTicks}";

        string groupKeyExpression(string tableAlias)
            => calculation switch
            {
                DayCalculationType.SumWeek or DayCalculationType.AverageWeek or DayCalculationType.MedianWeek => WeekStartKey(tableAlias + nameof(Day.SelectedDateTicks)),
                DayCalculationType.SumMonth or DayCalculationType.AverageMonth or DayCalculationType.MedianMonth => MonthKey(tableAlias + nameof(Day.SelectedDateTicks)),
                DayCalculationType.SumYear or DayCalculationType.AverageYear or DayCalculationType.MedianYear => YearKey(tableAlias + nameof(Day.SelectedDateTicks)),
                DayCalculationType.SumDayOfWeek or DayCalculationType.AverageDayOfWeek or DayCalculationType.MedianDayOfWeek => DayOfWeekKey(tableAlias + nameof(Day.SelectedDateTicks)),
                _ => "'ALL'",
            };

        string groupKey = groupKeyExpression("d.");

        string productJsonForGroup(string groupExpression, string outerGroupExpression, string aggregateKind)
        {
            if (aggregateKind == "SUM")
            {
                return $@"
                    (
                        SELECT json_group_array(
                            json_object(
                                '{nameof(Product.ProductNameId)}', pg.{nameof(Product.ProductNameId)},
                                '{nameof(Product.Number)}', pg.{nameof(Product.Number)},
                                '{nameof(Product.NumberEdit)}', pg.{nameof(Product.NumberEdit)},
                                '{nameof(Product.NumberReturn)}', pg.{nameof(Product.NumberReturn)},
                                '{nameof(Product.PriceTotal)}', pg.{nameof(Product.PriceTotal)},
                                '{nameof(Product.PriceTotalCorrect)}', pg.{nameof(Product.PriceTotalCorrect)},
                                '{nameof(Product.PriceTotalAfterCorrect)}', pg.{nameof(Product.PriceTotalAfterCorrect)}
                            )
                        )
                        FROM (
                            SELECT
                                p.{nameof(Product.ProductNameId)} AS {nameof(Product.ProductNameId)},
                                COALESCE(SUM(p.{nameof(Product.Number)}), 0) AS {nameof(Product.Number)},
                                COALESCE(SUM(p.{nameof(Product.NumberEdit)}), 0) AS {nameof(Product.NumberEdit)},
                                COALESCE(SUM(p.{nameof(Product.NumberReturn)}), 0) AS {nameof(Product.NumberReturn)},
                                COALESCE(SUM(p.{nameof(Product.PriceTotal)}), 0) AS {nameof(Product.PriceTotal)},
                                COALESCE(SUM(p.{nameof(Product.PriceTotalCorrect)}), 0) AS {nameof(Product.PriceTotalCorrect)},
                                COALESCE(SUM(p.{nameof(Product.PriceTotalAfterCorrect)}), 0) AS {nameof(Product.PriceTotalAfterCorrect)}
                            FROM {nameof(Product)} p
                            INNER JOIN {nameof(Day)} d2 ON d2.{nameof(Day.Id)} = p.{nameof(Product.DayId)}
                            GROUP BY p.{nameof(Product.ProductNameId)}
                        ) pg
                    ) AS JsonProducts";
            }

            if (aggregateKind == "AVG")
            {
                return $@"
                    (
                        SELECT json_group_array(
                            json_object(
                                '{nameof(Product.ProductNameId)}', pg.{nameof(Product.ProductNameId)},
                                '{nameof(Product.Number)}', pg.{nameof(Product.Number)},
                                '{nameof(Product.NumberEdit)}', pg.{nameof(Product.NumberEdit)},
                                '{nameof(Product.NumberReturn)}', pg.{nameof(Product.NumberReturn)},
                                '{nameof(Product.PriceTotal)}', pg.{nameof(Product.PriceTotal)},
                                '{nameof(Product.PriceTotalCorrect)}', pg.{nameof(Product.PriceTotalCorrect)},
                                '{nameof(Product.PriceTotalAfterCorrect)}', pg.{nameof(Product.PriceTotalAfterCorrect)}
                            )
                        )
                        FROM (
                            SELECT
                                p.{nameof(Product.ProductNameId)} AS {nameof(Product.ProductNameId)},
                                COALESCE(AVG(p.{nameof(Product.Number)}), 0) AS {nameof(Product.Number)},
                                COALESCE(AVG(p.{nameof(Product.NumberEdit)}), 0) AS {nameof(Product.NumberEdit)},
                                COALESCE(AVG(p.{nameof(Product.NumberReturn)}), 0) AS {nameof(Product.NumberReturn)},
                                COALESCE(AVG(p.{nameof(Product.PriceTotal)}), 0) AS {nameof(Product.PriceTotal)},
                                COALESCE(AVG(p.{nameof(Product.PriceTotalCorrect)}), 0) AS {nameof(Product.PriceTotalCorrect)},
                                COALESCE(AVG(p.{nameof(Product.PriceTotalAfterCorrect)}), 0) AS {nameof(Product.PriceTotalAfterCorrect)}
                            FROM {nameof(Product)} p
                            INNER JOIN {nameof(Day)} d2 ON d2.{nameof(Day.Id)} = p.{nameof(Product.DayId)}
                            GROUP BY p.{nameof(Product.ProductNameId)}
                        ) pg
                    ) AS JsonProducts";
            }

            return $@"
                (
                    SELECT json_group_array(
                        json_object(
                            '{nameof(Product.ProductNameId)}', pg.{nameof(Product.ProductNameId)},
                            '{nameof(Product.Number)}', pg.{nameof(Product.Number)},
                            '{nameof(Product.NumberEdit)}', pg.{nameof(Product.NumberEdit)},
                            '{nameof(Product.NumberReturn)}', pg.{nameof(Product.NumberReturn)},
                            '{nameof(Product.PriceTotal)}', pg.{nameof(Product.PriceTotal)},
                            '{nameof(Product.PriceTotalCorrect)}', pg.{nameof(Product.PriceTotalCorrect)},
                            '{nameof(Product.PriceTotalAfterCorrect)}', pg.{nameof(Product.PriceTotalAfterCorrect)}
                        )
                    )
                    FROM (
                        WITH ranked AS (
                            SELECT
                                p.{nameof(Product.ProductNameId)} AS {nameof(Product.ProductNameId)},
                                p.{nameof(Product.Number)} AS NumberValue,
                                p.{nameof(Product.NumberEdit)} AS NumberEditValue,
                                p.{nameof(Product.NumberReturn)} AS NumberReturnValue,
                                p.{nameof(Product.PriceTotal)} AS PriceTotalValue,
                                p.{nameof(Product.PriceTotalCorrect)} AS PriceTotalCorrectValue,
                                p.{nameof(Product.PriceTotalAfterCorrect)} AS PriceTotalAfterCorrectValue,
                                ROW_NUMBER() OVER (PARTITION BY p.{nameof(Product.ProductNameId)} ORDER BY p.{nameof(Product.Number)}) AS rnNumber,
                                COUNT(*) OVER (PARTITION BY p.{nameof(Product.ProductNameId)}) AS cntNumber,
                                ROW_NUMBER() OVER (PARTITION BY p.{nameof(Product.ProductNameId)} ORDER BY p.{nameof(Product.NumberEdit)}) AS rnNumberEdit,
                                COUNT(*) OVER (PARTITION BY p.{nameof(Product.ProductNameId)}) AS cntNumberEdit,
                                ROW_NUMBER() OVER (PARTITION BY p.{nameof(Product.ProductNameId)} ORDER BY p.{nameof(Product.NumberReturn)}) AS rnNumberReturn,
                                COUNT(*) OVER (PARTITION BY p.{nameof(Product.ProductNameId)}) AS cntNumberReturn,
                                ROW_NUMBER() OVER (PARTITION BY p.{nameof(Product.ProductNameId)} ORDER BY p.{nameof(Product.PriceTotal)}) AS rnPriceTotal,
                                COUNT(*) OVER (PARTITION BY p.{nameof(Product.ProductNameId)}) AS cntPriceTotal,
                                ROW_NUMBER() OVER (PARTITION BY p.{nameof(Product.ProductNameId)} ORDER BY p.{nameof(Product.PriceTotalCorrect)}) AS rnPriceTotalCorrect,
                                COUNT(*) OVER (PARTITION BY p.{nameof(Product.ProductNameId)}) AS cntPriceTotalCorrect,
                                ROW_NUMBER() OVER (PARTITION BY p.{nameof(Product.ProductNameId)} ORDER BY p.{nameof(Product.PriceTotalAfterCorrect)}) AS rnPriceTotalAfterCorrect,
                                COUNT(*) OVER (PARTITION BY p.{nameof(Product.ProductNameId)}) AS cntPriceTotalAfterCorrect
                            FROM {nameof(Product)} p
                            INNER JOIN {nameof(Day)} d2 ON d2.{nameof(Day.Id)} = p.{nameof(Product.DayId)}
                        )
                        SELECT
                            {nameof(Product.ProductNameId)},
                            COALESCE(AVG(CASE WHEN rnNumber IN ((cntNumber + 1) / 2, (cntNumber + 2) / 2) THEN NumberValue END), 0) AS {nameof(Product.Number)},
                            COALESCE(AVG(CASE WHEN rnNumberEdit IN ((cntNumberEdit + 1) / 2, (cntNumberEdit + 2) / 2) THEN NumberEditValue END), 0) AS {nameof(Product.NumberEdit)},
                            COALESCE(AVG(CASE WHEN rnNumberReturn IN ((cntNumberReturn + 1) / 2, (cntNumberReturn + 2) / 2) THEN NumberReturnValue END), 0) AS {nameof(Product.NumberReturn)},
                            COALESCE(AVG(CASE WHEN rnPriceTotal IN ((cntPriceTotal + 1) / 2, (cntPriceTotal + 2) / 2) THEN PriceTotalValue END), 0) AS {nameof(Product.PriceTotal)},
                            COALESCE(AVG(CASE WHEN rnPriceTotalCorrect IN ((cntPriceTotalCorrect + 1) / 2, (cntPriceTotalCorrect + 2) / 2) THEN PriceTotalCorrectValue END), 0) AS {nameof(Product.PriceTotalCorrect)},
                            COALESCE(AVG(CASE WHEN rnPriceTotalAfterCorrect IN ((cntPriceTotalAfterCorrect + 1) / 2, (cntPriceTotalAfterCorrect + 2) / 2) THEN PriceTotalAfterCorrectValue END), 0) AS {nameof(Product.PriceTotalAfterCorrect)}
                        FROM ranked
                        GROUP BY {nameof(Product.ProductNameId)}
                    ) pg
                ) AS JsonProducts";
        }

        return calculation switch
        {
            DayCalculationType.None => $@"
                SELECT
                    d.*,
                    (
                        SELECT json_group_array(
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
                                '{nameof(Product.UserUpdatedId)}', p.{nameof(Product.UserUpdatedId)}
                            )
                        )
                        FROM {nameof(Product)} p
                        WHERE p.{nameof(Product.DayId)} = d.{nameof(Day.Id)}
                          AND p.{nameof(Product.IsDelete)} = 0
                    ) AS JsonProducts,
                    (
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
                        WHERE c.{nameof(Cake.DayId)} = d.{nameof(Day.Id)}
                          AND c.{nameof(Cake.IsDelete)} = 0
                    ) AS JsonCakes
                FROM {nameof(Day)} d
                {where}
                ORDER BY d.{nameof(Day.SelectedDateTicks)};",

            DayCalculationType.SumWeek or DayCalculationType.SumMonth or DayCalculationType.SumYear or DayCalculationType.SumDayOfWeek or DayCalculationType.SumAll => $@"
                SELECT
                    {groupKey} AS GroupKey,
                    COALESCE(SUM(d.{nameof(Day.TotalPrice)}), 0) AS {nameof(Day.TotalPrice)},
                    COALESCE(SUM(d.{nameof(Day.TotalPriceProducts)}), 0) AS {nameof(Day.TotalPriceProducts)},
                    COALESCE(SUM(d.{nameof(Day.TotalPriceCake)}), 0) AS {nameof(Day.TotalPriceCake)},
                    COALESCE(SUM(d.{nameof(Day.TotalPriceCorrect)}), 0) AS {nameof(Day.TotalPriceCorrect)},
                    COALESCE(SUM(d.{nameof(Day.TotalPriceAfterCorrect)}), 0) AS {nameof(Day.TotalPriceAfterCorrect)},
                    COALESCE(SUM(d.{nameof(Day.TotalPriceMoney)}), 0) AS {nameof(Day.TotalPriceMoney)},
                    COALESCE(SUM(d.{nameof(Day.TotalPriceDifference)}), 0) AS {nameof(Day.TotalPriceDifference)},
                    {productJsonForGroup(groupKey, groupKey, "SUM")}
                FROM {nameof(Day)} d
                {where}
                GROUP BY {groupKey}
                ORDER BY GroupKey;",

            DayCalculationType.AverageWeek or DayCalculationType.AverageMonth or DayCalculationType.AverageYear or DayCalculationType.AverageDayOfWeek or DayCalculationType.AverageAll => $@"
                SELECT
                    {groupKey} AS GroupKey,
                    COALESCE(AVG(d.{nameof(Day.TotalPrice)}), 0) AS {nameof(Day.TotalPrice)},
                    COALESCE(AVG(d.{nameof(Day.TotalPriceProducts)}), 0) AS {nameof(Day.TotalPriceProducts)},
                    COALESCE(AVG(d.{nameof(Day.TotalPriceCake)}), 0) AS {nameof(Day.TotalPriceCake)},
                    COALESCE(AVG(d.{nameof(Day.TotalPriceCorrect)}), 0) AS {nameof(Day.TotalPriceCorrect)},
                    COALESCE(AVG(d.{nameof(Day.TotalPriceAfterCorrect)}), 0) AS {nameof(Day.TotalPriceAfterCorrect)},
                    COALESCE(AVG(d.{nameof(Day.TotalPriceMoney)}), 0) AS {nameof(Day.TotalPriceMoney)},
                    COALESCE(AVG(d.{nameof(Day.TotalPriceDifference)}), 0) AS {nameof(Day.TotalPriceDifference)},
                    {productJsonForGroup(groupKey, groupKey, "AVG")}
                FROM {nameof(Day)} d
                {where}
                GROUP BY {groupKey}
                ORDER BY GroupKey;",

            DayCalculationType.MedianWeek or DayCalculationType.MedianMonth or DayCalculationType.MedianYear or DayCalculationType.MedianDayOfWeek or DayCalculationType.MedianAll => $@"
                WITH ranked AS (
                    SELECT
                        {groupKey} AS GroupKey,
                        d.{nameof(Day.TotalPrice)} AS TotalPrice,
                        d.{nameof(Day.TotalPriceProducts)} AS TotalPriceProducts,
                        d.{nameof(Day.TotalPriceCake)} AS TotalPriceCake,
                        d.{nameof(Day.TotalPriceCorrect)} AS TotalPriceCorrect,
                        d.{nameof(Day.TotalPriceAfterCorrect)} AS TotalPriceAfterCorrect,
                        d.{nameof(Day.TotalPriceMoney)} AS TotalPriceMoney,
                        d.{nameof(Day.TotalPriceDifference)} AS TotalPriceDifference,
                        ROW_NUMBER() OVER (PARTITION BY {groupKey} ORDER BY d.{nameof(Day.TotalPrice)}) AS rnTotalPrice,
                        COUNT(*) OVER (PARTITION BY {groupKey}) AS cntTotalPrice,
                        ROW_NUMBER() OVER (PARTITION BY {groupKey} ORDER BY d.{nameof(Day.TotalPriceProducts)}) AS rnTotalPriceProducts,
                        COUNT(*) OVER (PARTITION BY {groupKey}) AS cntTotalPriceProducts,
                        ROW_NUMBER() OVER (PARTITION BY {groupKey} ORDER BY d.{nameof(Day.TotalPriceCake)}) AS rnTotalPriceCake,
                        COUNT(*) OVER (PARTITION BY {groupKey}) AS cntTotalPriceCake,
                        ROW_NUMBER() OVER (PARTITION BY {groupKey} ORDER BY d.{nameof(Day.TotalPriceCorrect)}) AS rnTotalPriceCorrect,
                        COUNT(*) OVER (PARTITION BY {groupKey}) AS cntTotalPriceCorrect,
                        ROW_NUMBER() OVER (PARTITION BY {groupKey} ORDER BY d.{nameof(Day.TotalPriceAfterCorrect)}) AS rnTotalPriceAfterCorrect,
                        COUNT(*) OVER (PARTITION BY {groupKey}) AS cntTotalPriceAfterCorrect,
                        ROW_NUMBER() OVER (PARTITION BY {groupKey} ORDER BY d.{nameof(Day.TotalPriceMoney)}) AS rnTotalPriceMoney,
                        COUNT(*) OVER (PARTITION BY {groupKey}) AS cntTotalPriceMoney,
                        ROW_NUMBER() OVER (PARTITION BY {groupKey} ORDER BY d.{nameof(Day.TotalPriceDifference)}) AS rnTotalPriceDifference,
                        COUNT(*) OVER (PARTITION BY {groupKey}) AS cntTotalPriceDifference
                    FROM {nameof(Day)} d
                    {where}
                )
                SELECT
                    GroupKey,
                    COALESCE(AVG(CASE WHEN rnTotalPrice IN ((cntTotalPrice + 1) / 2, (cntTotalPrice + 2) / 2) THEN TotalPrice END), 0) AS {nameof(Day.TotalPrice)},
                    COALESCE(AVG(CASE WHEN rnTotalPriceProducts IN ((cntTotalPriceProducts + 1) / 2, (cntTotalPriceProducts + 2) / 2) THEN TotalPriceProducts END), 0) AS {nameof(Day.TotalPriceProducts)},
                    COALESCE(AVG(CASE WHEN rnTotalPriceCake IN ((cntTotalPriceCake + 1) / 2, (cntTotalPriceCake + 2) / 2) THEN TotalPriceCake END), 0) AS {nameof(Day.TotalPriceCake)},
                    COALESCE(AVG(CASE WHEN rnTotalPriceCorrect IN ((cntTotalPriceCorrect + 1) / 2, (cntTotalPriceCorrect + 2) / 2) THEN TotalPriceCorrect END), 0) AS {nameof(Day.TotalPriceCorrect)},
                    COALESCE(AVG(CASE WHEN rnTotalPriceAfterCorrect IN ((cntTotalPriceAfterCorrect + 1) / 2, (cntTotalPriceAfterCorrect + 2) / 2) THEN TotalPriceAfterCorrect END), 0) AS {nameof(Day.TotalPriceAfterCorrect)},
                    COALESCE(AVG(CASE WHEN rnTotalPriceMoney IN ((cntTotalPriceMoney + 1) / 2, (cntTotalPriceMoney + 2) / 2) THEN TotalPriceMoney END), 0) AS {nameof(Day.TotalPriceMoney)},
                    COALESCE(AVG(CASE WHEN rnTotalPriceDifference IN ((cntTotalPriceDifference + 1) / 2, (cntTotalPriceDifference + 2) / 2) THEN TotalPriceDifference END), 0) AS {nameof(Day.TotalPriceDifference)},
                    {productJsonForGroup(groupKey, groupKey, "MEDIAN")}
                FROM ranked
                GROUP BY GroupKey
                ORDER BY GroupKey;",

            _ => $@"
                SELECT
                    d.*,
                    (
                        SELECT json_group_array(
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
                                '{nameof(Product.UserUpdatedId)}', p.{nameof(Product.UserUpdatedId)}
                            )
                        )
                        FROM {nameof(Product)} p
                        WHERE p.{nameof(Product.DayId)} = d.{nameof(Day.Id)}
                          AND p.{nameof(Product.IsDelete)} = 0
                    ) AS JsonProducts,
                    (
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
                        WHERE c.{nameof(Cake.DayId)} = d.{nameof(Day.Id)}
                          AND c.{nameof(Cake.IsDelete)} = 0
                    ) AS JsonCakes
                FROM {nameof(Day)} d
                {where}
                ORDER BY d.{nameof(Day.SelectedDateTicks)};"
        };
    }

    }
