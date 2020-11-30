﻿using System;

namespace ETLBox.DataFlow
{
    /// <summary>
    /// This attribute defines a column name to which the value of the property is mapped when writing or reading
    /// from a database or using the ColumnRename transformation.
    /// By default, when reading from a database source the mapping which property stores data of which column
    /// is resolved by the property names. Using this attribute, you can specify the column name that maps to a property.
    /// In the ColumnRename transformation, this property is used to rename a column. 
    /// </summary>
    /// <example>
    ///  public class MyPoco
    /// {
    ///     [ColumnMap("Column1")]
    ///     public string MyProperty { get; set; }
    /// }
    /// </example>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class ColumnMap : Attribute
    {
        /// <summary>
        /// Name of the column in the database or the new name when renaming
        /// </summary>
        public string NewName { get; set; }

        public ColumnMap()
        {

        }

        /// <summary>
        /// Create a mapping between the current property and a column        
        /// </summary>
        /// <param name="newName">Name of the column in the database or the new name when renaming</param>
        public ColumnMap(string newName)
        {
            NewName = newName;
        }

        /// <summary>
        /// Index of the element in the array
        /// </summary>
        public int? ArrayIndex { get; set; }

        /// <summary>
        /// Current name of the column or property
        /// </summary>
        public string CurrentName { get; set; }

        /// <summary>
        /// If set to true, this column is left out when using the ColumnRename transformation.
        /// </summary>
        public bool RemoveColumn { get; set; }
    }
}
