using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Reflection;
using System.Windows.Forms;

using RapidSoft.Etl.Runtime.Functions;

namespace RapidSoft.Etl.Editor
{
    public class EtlModelCollectionEditor : CollectionEditor
    {
        #region Constructors

        public EtlModelCollectionEditor(Type type) 
            :base(type)
        {
        }

        #endregion

        #region Events

        public static event PropertyValueChangedEventHandler AnyPropertyValueChanged;

        #endregion

        #region Methods

        protected override object CreateInstance(Type itemType)
        {
            if (itemType.IsAbstract)
            {
                return base.CreateInstance(itemType);
            }
            else
            {
                var obj = Activator.CreateInstance(itemType);
                return obj;
            }
        }

        protected override Type[] CreateNewItemTypes()
        {
            if (this.CollectionItemType.IsSealed)
            {
                return base.CreateNewItemTypes();
            }
            else
            {
                var assembly = this.CollectionItemType.Assembly;
                var types = new List<Type>();
                AddTypes(this.CollectionItemType.Assembly, types);

                types.Sort(delegate(Type left, Type right)
                {
                    return string.Compare(left.Name, right.Name);
                });

                if (types.Count > 0)
                {
                    return types.ToArray();
                }
                else
                {
                    return base.CreateNewItemTypes();
                }
            }
        }

        private void AddTypes(Assembly assembly, List<Type> types)
        {
            foreach (var type in assembly.GetTypes())
            {
                if (this.CollectionItemType.IsAssignableFrom(type) && !type.IsAbstract)
                {
                    types.Add(type);
                }
            }
        }

        protected override CollectionForm CreateCollectionForm()
        {
            var collectionForm = base.CreateCollectionForm();
            var frmCollectionEditorForm = collectionForm as Form;
            var tlpLayout = frmCollectionEditorForm.Controls[0] as TableLayoutPanel;

            const int innerPropertyGridIndex = 5;

            if (tlpLayout != null && tlpLayout.Controls.Count > innerPropertyGridIndex)
            {
                if (tlpLayout.Controls[innerPropertyGridIndex] is PropertyGrid)
                {
                    var propertyGrid = (PropertyGrid)tlpLayout.Controls[innerPropertyGridIndex];
                    propertyGrid.PropertyValueChanged += new PropertyValueChangedEventHandler(propertyGrid_PropertyValueChanged);
                }
            }

            return collectionForm;
        }

        private void propertyGrid_PropertyValueChanged(object sender, PropertyValueChangedEventArgs e)
        {
            if (EtlModelCollectionEditor.AnyPropertyValueChanged != null)
            {
                EtlModelCollectionEditor.AnyPropertyValueChanged(this, e);
            }
        }

        #endregion
    }
}