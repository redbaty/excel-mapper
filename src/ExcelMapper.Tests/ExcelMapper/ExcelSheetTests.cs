using Xunit;

namespace ExcelMapper.Tests
{
    public class ExcelSheetTests
    {
        [Fact]
        public void ReadHeading_HasHeading_ReturnsExpected()
        {
            using (var importer = Helpers.GetImporter("Primitives.xlsx"))
            {
                ExcelSheet sheet = importer.ReadSheet();
                ExcelHeading heading = sheet.ReadHeading();
                Assert.Same(heading, sheet.Heading);

                Assert.Equal(new string[] { "Int Value", "StringValue", "Bool Value", "Enum Value" }, heading.ColumnNames);
            }
        }

        [Fact]
        public void ReadHeading_AlreadyReadHeading_ThrowsExcelMappingException()
        {
            using (var importer = Helpers.GetImporter("Primitives.xlsx"))
            {
                ExcelSheet sheet = importer.ReadSheet();
                sheet.ReadHeading();

                Assert.Throws<ExcelMappingException>(() => sheet.ReadHeading());
            }
        }

        [Fact]
        public void ReadHeading_DoesNotHaveHasHeading_ThrowsExcelMappingException()
        {
            using (var importer = Helpers.GetImporter("Primitives.xlsx"))
            {
                importer.Configuration.HasHeading = _ => false;
                ExcelSheet sheet = importer.ReadSheet();

                Assert.Throws<ExcelMappingException>(() => sheet.ReadHeading());
            }
        }

        [Fact]
        public void ReadRow_NoSuchMapping_ThrowsExcelMappingException()
        {
            using (var importer = Helpers.GetImporter("Primitives.xlsx"))
            {
                ExcelSheet sheet = importer.ReadSheet();
                sheet.ReadHeading();

                Assert.Throws<ExcelMappingException>(() => sheet.ReadRow<PrimitiveSheet1>());
            }
        }

        [Fact]
        public void ReadRow_FirstRow_ReturnsExpected()
        {
            using (var importer = Helpers.GetImporter("Primitives.xlsx"))
            {
                importer.Configuration.RegisterMapping<PrimitiveSheet1Mapping>();

                ExcelSheet sheet = importer.ReadSheet();
                sheet.ReadHeading();

                PrimitiveSheet1 row = sheet.ReadRow<PrimitiveSheet1>();
                Assert.Equal(1, row.IntValue);
                Assert.Equal("a", row.StringValue);
                Assert.True(row.BoolValue);
                Assert.Equal(PrimitiveSheet1Enum.Government, row.EnumValue);
            }
        }

        public class PrimitiveSheet1
        {
            public int IntValue { get; set; }
            public string StringValue { get; set; }
            public bool BoolValue { get; set; }
            public PrimitiveSheet1Enum EnumValue { get; set; }
        }

        public enum PrimitiveSheet1Enum
        {
            Unknown,
            Government,
            NGO
        }

        public class PrimitiveSheet1Mapping : ExcelClassMap<PrimitiveSheet1>
        {
            public PrimitiveSheet1Mapping() : base()
            {
                Map(p => p.IntValue).WithColumnName("Int Value");
                Map(p => p.StringValue);
                Map(p => p.BoolValue).WithIndex(2);
                Map(p => p.EnumValue).WithColumnName("Enum Value");
            }
        }
    }
}
