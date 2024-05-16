// Licensed to the .NET Foundation under one or more agreements.

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ElectronApp.Database.Extensions
{
    /// <summary>
    /// 提供將屬性慣例應用於模型生成器的擴展方法。
    /// Provides extension methods for applying attribute conventions to the model builder.
    /// </summary>
    public static class SqlDefaultValueAttributeConvention
    {
        /// <summary>
        /// 將 SQL 預設值屬性慣例應用於模型生成器。
        /// Applies the SQL default value attribute convention to the model builder.
        /// </summary>
        /// <param name="builder">The model builder.</param>
        public static void Apply(ModelBuilder builder)
        {
            ConventionBehaviors
                .SetSqlValueForPropertiesWithAttribute<SqlDefaultValueAttribute>(builder, x => x.DefaultValue);
        }
    }

    /// <summary>
    /// 提供將屬性慣例應用於模型生成器的擴展方法。
    /// Provides extension methods for applying attribute conventions to the model builder.
    /// </summary>
    public static class DecimalPrecisionAttributeConvention
    {
        /// <summary>
        /// 將十進制精度屬性慣例應用於模型生成器。
        /// Applies the decimal precision attribute convention to the model builder.
        /// </summary>
        /// <param name="builder">The model builder.</param>
        public static void Apply(ModelBuilder builder)
        {
            ConventionBehaviors
                .SetTypeForPropertiesWithAttribute<DecimalPrecisionAttribute>(builder,
                    x => $"decimal({x.Precision}, {x.Scale})");
        }
    }

    /// <summary>
    /// 提供將屬性慣例應用於模型生成器的擴展方法。
    /// Provides extension methods for applying attribute conventions to the model builder.
    /// </summary>
    public class CustomDataTypeAttributeConvention
    {
        /// <summary>
        /// 將自訂資料類型屬性慣例應用於模型生成器。
        /// Applies the custom data type attribute convention to the model builder.
        /// </summary>
        /// <param name="builder">The model builder.</param>
        public static void Apply(ModelBuilder builder)
        {
            ConventionBehaviors
                .SetTypeForPropertiesWithAttribute<DataTypeAttribute>(builder,
                    x => x?.CustomDataType ?? string.Empty);
        }
    }

    /// <summary>
    /// 提供將屬性慣例應用於模型生成器的輔助方法。
    /// Provides helper methods for applying attribute conventions to the model builder.
    /// </summary>
    public static class ConventionBehaviors
    {
        /// <summary>
        /// 為具有指定屬性的屬性設置欄位類型。
        /// Sets the column type for properties with the specified attribute.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <param name="builder">The model builder.</param>
        /// <param name="lambda">The lambda expression to get the column type from the attribute.</param>
        public static void SetTypeForPropertiesWithAttribute<TAttribute>(ModelBuilder builder, Func<TAttribute, string> lambda) where TAttribute : class
        {
            SetPropertyValue<TAttribute>(builder).ForEach((x) =>
            {
                x.Item1.SetColumnType(lambda(x.Item2));
            });
        }

        /// <summary>
        /// 為具有指定屬性的屬性設置預設值 SQL。
        /// Sets the default value SQL for properties with the specified attribute.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <param name="builder">The model builder.</param>
        /// <param name="lambda">The lambda expression to get the default value SQL from the attribute.</param>
        public static void SetSqlValueForPropertiesWithAttribute<TAttribute>(ModelBuilder builder, Func<TAttribute, string> lambda) where TAttribute : class
        {
            SetPropertyValue<TAttribute>(builder).ForEach((x) =>
            {
                x.Item1.SetDefaultValueSql(lambda(x.Item2));
            });
        }

        private static List<Tuple<IMutableProperty, TAttribute>> SetPropertyValue<TAttribute>(ModelBuilder builder) where TAttribute : class
        {
            var propsToModify = new List<Tuple<IMutableProperty, TAttribute>>();
            foreach (var entity in builder.Model.GetEntityTypes())
            {
                var properties = entity.GetProperties();
                foreach (var property in properties)
                {
                    var attribute = property.PropertyInfo
                        ?.GetCustomAttributes(typeof(TAttribute), false)
                        .FirstOrDefault() as TAttribute;
                    if (attribute != null)
                    {
                        propsToModify.Add(new Tuple<IMutableProperty, TAttribute>(property, attribute));
                    }
                }
            }
            return propsToModify;
        }
    }

    /// <summary>
    /// 指定屬性的 SQL 伺服器上定義的預設值。
    /// Specifies a default value defined on the SQL server for a property.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class SqlDefaultValueAttribute : Attribute
    {
        /// <summary>
        /// 獲取或設置要應用的預設值。
        /// Gets or sets the default value to apply.
        /// </summary>
        public string DefaultValue { get; set; }

        /// <summary>
        /// 初始化 <see cref="SqlDefaultValueAttribute"/> 類的新執行個體。
        /// Initializes a new instance of the <see cref="SqlDefaultValueAttribute"/> class.
        /// </summary>
        /// <param name="value">The default value to apply.</param>
        public SqlDefaultValueAttribute(string value)
        {
            DefaultValue = value;
        }
    }

    /// <summary>
    /// 指定十進制 SQL 資料類型的精度。
    /// Specifies the decimal precision of a decimal SQL data type.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class DecimalPrecisionAttribute : Attribute
    {
        /// <summary>
        /// 獲取或設置精度 - 即小數點左右的位數。
        /// Gets or sets the precision - the number of digits both left and right of the decimal.
        /// </summary>
        public int Precision { get; set; }

        /// <summary>
        /// 獲取或設置縮放 - 即小數點右側的位數。
        /// Gets or sets the scale - the number of digits to the right of the decimal.
        /// </summary>
        public int Scale { get; set; }

        /// <summary>
        /// 初始化 <see cref="DecimalPrecisionAttribute"/> 類的新執行個體。
        /// Initializes a new instance of the <see cref="DecimalPrecisionAttribute"/> class.
        /// </summary>
        /// <param name="precision">The precision - the number of digits both left and right of the decimal.</param>
        /// <param name="scale">The scale - the number of digits to the right of the decimal.</param>
        public DecimalPrecisionAttribute(int precision, int scale)
        {
            Precision = precision;
            Scale = scale;
        }

        /// <summary>
        /// 初始化 <see cref="DecimalPrecisionAttribute"/> 類的新執行個體。
        /// Initializes a new instance of the <see cref="DecimalPrecisionAttribute"/> class.
        /// </summary>
        /// <param name="values">The array of values where the first element is the precision and the second element is the scale.</param>
        public DecimalPrecisionAttribute(int[] values)
        {
            Precision = values[0];
            Scale = values[1];
        }
    }
}
