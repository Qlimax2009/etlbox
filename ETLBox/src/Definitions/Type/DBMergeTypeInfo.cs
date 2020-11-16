﻿using System;
using System.Collections.Generic;
using System.Reflection;

namespace ETLBox.DataFlow
{
    internal class DBMergeTypeInfo : TypeInfo
    {
        internal List<string> IdColumnNames { get; set; } = new List<string>();
        internal List<string> CompareColumnNames { get; set; } = new List<string>();
        internal List<PropertyInfo> IdAttributeProps { get; } = new List<PropertyInfo>();
        internal List<PropertyInfo> CompareAttributeProps { get; } = new List<PropertyInfo>();
        internal List<Tuple<PropertyInfo, object>> DeleteAttributeProps { get; } = new List<Tuple<PropertyInfo, object>>();
        internal PropertyInfo ChangeDateProperty { get; set; }
        internal PropertyInfo ChangeActionProperty { get; set; }
        internal MergeProperties MergeProps { get; set; }

        internal DBMergeTypeInfo(Type typ, MergeProperties mergeProps) : base(typ)
        {
            MergeProps = mergeProps;
            GatherTypeInfo();
        }

        protected override void RetrieveAdditionalTypeInfo(PropertyInfo propInfo, int currentIndex)
        {
            AddMergeIdColumnNameAttribute(propInfo);
            AddMergeCompareColumnNameAttribute(propInfo);
            AddIdAddAttributeProps(propInfo);
            AddCompareAttributeProps(propInfo);
            AddDeleteAttributeProps(propInfo);
            AddChangeActionProp(propInfo);
            AddChangeDateProp(propInfo);
        }

        private void AddMergeIdColumnNameAttribute(PropertyInfo propInfo)
        {
            if (MergeProps.IdPropertyNames.Contains(propInfo.Name))
                IdColumnNames.Add(propInfo.Name);
            else
            {
                var attr = propInfo.GetCustomAttribute(typeof(IdColumn)) as IdColumn;
                if (attr != null)
                {
                    var cmattr = propInfo.GetCustomAttribute(typeof(ColumnMap)) as ColumnMap;
                    if (cmattr != null)
                        IdColumnNames.Add(cmattr.NewName);
                    else
                        IdColumnNames.Add(propInfo.Name);
                }
            }
        }

        private void AddMergeCompareColumnNameAttribute(PropertyInfo propInfo)
        {
            if (MergeProps.ComparePropertyNames.Contains(propInfo.Name))
                CompareColumnNames.Add(propInfo.Name);
            else
            {
                var attr = propInfo.GetCustomAttribute(typeof(CompareColumn)) as CompareColumn;
                if (attr != null)
                {
                    var cmattr = propInfo.GetCustomAttribute(typeof(ColumnMap)) as ColumnMap;
                    if (cmattr != null)
                        CompareColumnNames.Add(cmattr.NewName);
                    else
                        CompareColumnNames.Add(propInfo.Name);
                }
            }
        }

        private void AddIdAddAttributeProps(PropertyInfo propInfo)
        {
            if (MergeProps.IdPropertyNames.Contains(propInfo.Name))
                IdAttributeProps.Add(propInfo);
            else
            {
                var idAttr = propInfo.GetCustomAttribute(typeof(IdColumn)) as IdColumn;
                if (idAttr != null)
                    IdAttributeProps.Add(propInfo);
            }
        }

        private void AddCompareAttributeProps(PropertyInfo propInfo)
        {
            if (MergeProps.ComparePropertyNames.Contains(propInfo.Name))
                CompareAttributeProps.Add(propInfo);
            else
            {
                var compAttr = propInfo.GetCustomAttribute(typeof(CompareColumn)) as CompareColumn;
                if (compAttr != null)
                    CompareAttributeProps.Add(propInfo);
            }
        }

        private void AddDeleteAttributeProps(PropertyInfo propInfo)
        {
            if (MergeProps.DeletionProperties.ContainsKey(propInfo.Name))
                DeleteAttributeProps.Add(Tuple.Create(propInfo, MergeProps.DeletionProperties[propInfo.Name]));
            else
            {
                var deleteAttr = propInfo.GetCustomAttribute(typeof(DeleteColumn)) as DeleteColumn;
                if (deleteAttr != null)
                    DeleteAttributeProps.Add(Tuple.Create(propInfo, deleteAttr.DeleteOnMatchValue));
            }
        }
        private void AddChangeActionProp(PropertyInfo propInfo)
        {
            if (propInfo.Name.ToLower() == MergeProps.ChangeActionPropertyName.ToLower())
                ChangeActionProperty = propInfo;
        }

        private void AddChangeDateProp(PropertyInfo propInfo)
        {
            if (propInfo.Name.ToLower() == MergeProps.ChangeDatePropertyName.ToLower())
                ChangeDateProperty = propInfo;
        }

    }
}

