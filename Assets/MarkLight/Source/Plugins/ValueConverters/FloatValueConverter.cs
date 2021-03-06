#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.UI;
using UnityEngine;
using System.Globalization;
#endregion

namespace MarkLight.ValueConverters
{
    /// <summary>
    /// Value converter for float type.
    /// </summary>
    public class FloatValueConverter : ValueConverter
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FloatValueConverter()
        {
            _type = typeof(float);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Value converter for float type.
        /// </summary>
        public override ConversionResult Convert(object value, ValueConverterContext context)
        {
            if (value == null)
            {
                return base.Convert(value, context);
            }

            Type valueType = value.GetType();
            if (valueType == _type)
            {
                return base.Convert(value, context);
            }
            else if (valueType == _stringType)
            {
                var stringValue = (string)value;
                try
                {
                    var convertedValue = System.Convert.ToSingle(stringValue, CultureInfo.InvariantCulture);
                    return new ConversionResult(convertedValue);
                }
                catch (Exception e)
                {
                    return ConversionFailed(value, e);
                }
            }
            else
            {
                // attempt to convert using system type converter
                try
                {
                    var convertedValue = System.Convert.ToSingle(value, CultureInfo.InvariantCulture);
                    return new ConversionResult(convertedValue);
                }
                catch (Exception e)
                {
                    return ConversionFailed(value, e);
                }
            }
        }

        /// <summary>
        /// Converts value to string.
        /// </summary>
        public override string ConvertToString(object value)
        {
            return value.ToString();
        }

        #endregion
    }
}
