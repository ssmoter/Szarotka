using FluentAssertions;

namespace InventoryUnitTest.ModelTest
{
    public class ProductNameAndPriceOriginalComparison
    {
        private static readonly string ProductJson = "[{\"name\":\"Babka du\\u017Ca\",\"idName\":\"e1c7d127-8f86-49e4-bbdf-26b035d893f6\",\"idPrice\":\"323ecff4-5b51-4218-9a0d-0dac28a94fce\",\"price\":1200},{\"name\":\"Babka Ma\\u0142a\",\"idName\":\"1d793cea-742c-4b6a-ad4a-cb567a64bf1a\",\"idPrice\":\"2bad3bff-1f81-4ced-8215-d97c2bfebbae\",\"price\":300},{\"name\":\"Babka \\u015Brednia\",\"idName\":\"7112cea4-6f83-4cfe-9175-d425c94d1651\",\"idPrice\":\"a3db562d-2943-4873-a94d-9c870509daaa\",\"price\":800},{\"name\":\"Baranek\",\"idName\":\"60acb83d-3141-4f7d-9d15-714ad357a88d\",\"idPrice\":\"440c664c-3fb3-48b4-bd94-6e0458910f80\",\"price\":300},{\"name\":\"Bu\\u0142ka\",\"idName\":\"e5226138-0e10-9f4c-cd7b-b91b87ef9f24\",\"idPrice\":\"c884e0af-680a-aa9f-2398-50e032390792\",\"price\":150},{\"name\":\"Bu\\u0142ka ma\\u015Blana\",\"idName\":\"9295c14d-4b92-63c4-2650-895cf7afea7f\",\"idPrice\":\"1bfb70e3-eabf-e373-82d9-d001cd2689ac\",\"price\":150},{\"name\":\"Bu\\u0142ka tarta\",\"idName\":\"f5d38837-3bae-2d5b-5ff0-95e6cf365d46\",\"idPrice\":\"7f656b80-7b4e-d1d3-5b60-131acf77e6ed\",\"price\":350},{\"name\":\"Bu\\u0142ka z serem\",\"idName\":\"61b42773-0b9f-aa21-fe8a-8e19335fe799\",\"idPrice\":\"cd126ccd-8075-e15b-c580-d28b43693ccd\",\"price\":250},{\"name\":\"Cha\\u0142ka\",\"idName\":\"38422bb1-f816-33cb-4e63-29e4925da2e4\",\"idPrice\":\"c7dc4ee3-e4aa-0615-cc43-e554cfcddd88\",\"price\":500},{\"name\":\"Chleb\",\"idName\":\"d2034a6a-3a0d-9b49-05a3-bc49adfc329a\",\"idPrice\":\"8012d48e-5f9b-439f-a213-ed2e997b9432\",\"price\":600},{\"name\":\"Chleb do \\u015Bwiecenia\",\"idName\":\"3af7e393-b6af-4015-b5f7-c56d85f375a7\",\"idPrice\":\"5183158d-dea2-4f16-a227-647554417656\",\"price\":350},{\"name\":\"Chleb suchy\",\"idName\":\"fa93a809-fc49-99ec-f522-818e412a4184\",\"idPrice\":\"8bbf1b67-2d21-4ecf-9599-b39bca9a4187\",\"price\":250},{\"name\":\"Ciastka (opak. 400g)\",\"idName\":\"7af5a086-0e96-4fbd-a4f8-dec5818e70a0\",\"idPrice\":\"cdfb236f-30b1-ec4d-22f8-7fea28d6d072\",\"price\":1000},{\"name\":\"Ciastka francuskie\",\"idName\":\"94c617ee-9426-e4a9-e8e4-6286962cd4c1\",\"idPrice\":\"ac1c0158-013b-cdf5-153b-6cd313766481\",\"price\":300},{\"name\":\"Dro\\u017Cd\\u017C\\u00F3wka\",\"idName\":\"9bcca084-69d5-c245-4319-88acf6469c62\",\"idPrice\":\"51aa278f-a059-f31e-cbd3-9b1f39966694\",\"price\":250},{\"name\":\"Dro\\u017Cd\\u017C\\u00F3wki na wag\\u0119 (opak.)\",\"idName\":\"345ef3b2-0ab1-44d0-6347-2b0916c1fc1c\",\"idPrice\":\"c93c1ce9-4b9b-62d5-4eb1-0fb97bc39500\",\"price\":900},{\"name\":\"Du\\u017Cy chleb\",\"idName\":\"24ebc827-bb4e-6fac-4eb4-8f88f524eaf1\",\"idPrice\":\"3200c8fa-be20-449b-9a82-84ff2e9598b5\",\"price\":1200},{\"name\":\"Ko\\u0142acz\",\"idName\":\"03aaddc6-b550-d8cd-9fb7-b878bf69cdc0\",\"idPrice\":\"b118b4dc-c0ea-6710-1efb-e600fb17c8a0\",\"price\":500},{\"name\":\"Makowiec\",\"idName\":\"a1456c27-7a8e-1669-f9b1-89e3290ac9f2\",\"idPrice\":\"5c1ff8ac-e072-e9f7-10aa-a22d008bbb70\",\"price\":1400},{\"name\":\"Ma\\u0142a bu\\u0142ka\",\"idName\":\"944f2566-0502-fae1-39f1-3540c389cbe4\",\"idPrice\":\"87c365cd-8536-cf5f-c177-c177bd6c6d94\",\"price\":70},{\"name\":\"Mini Pizza\",\"idName\":\"f03d18f5-12ee-0106-00cd-fa25e92ddb24\",\"idPrice\":\"99ed587e-2499-dcbd-ddd3-428f1a137a25\",\"price\":300},{\"name\":\"Par\\u00F3wka w cie\\u015Bcie\",\"idName\":\"60edd453-e65a-4375-fdc0-3aa98fd60da6\",\"idPrice\":\"af0ffb76-fbad-bcd6-b502-994e27405de7\",\"price\":350},{\"name\":\"Piernik/Babka\",\"idName\":\"f500e080-5936-4a2a-43cd-074323304b86\",\"idPrice\":\"ef63cbcb-977a-55d0-5309-589e8fd226ce\",\"price\":1200},{\"name\":\"Precel\",\"idName\":\"4024c248-2c8e-4fcf-ac83-c3d3059b4322\",\"idPrice\":\"9956ad34-69ea-4bf1-8d32-4581c4f6c264\",\"price\":250},{\"name\":\"Wafle (opak. 400g)\",\"idName\":\"b71b38a6-aaeb-51f7-624d-f6c90dec2c49\",\"idPrice\":\"d289404c-2bcf-4b01-86d0-39721960fda5\",\"price\":1200}]";
        [Fact]
        public void ComparisonFromStaticValue()
        {
            var defaultProduct = DataBase.Data.InventoryTables.DefaultProducts;
            var defaultProductJson= System.Text.Json.JsonSerializer.Serialize(defaultProduct.OrderBy(x => x.Name.Name).Select(x => new
            {
                name = x.Name.Name,
                idName = x.Name.Id,
                idPrice = x.Price.Id,
                price = x.Price.Price,
            }));

            defaultProductJson.Should().Be(ProductJson);
        }

    }
}
