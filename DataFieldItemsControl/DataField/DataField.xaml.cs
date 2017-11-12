using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace KarveControls
{
    /// <summary>
    /// DataField Component Definition.
    /// </summary>
    public partial class DataField : UserControl
    {
        public static readonly RoutedEvent DataFieldChangedEvent =
            EventManager.RegisterRoutedEvent(
                "DataFieldChanged",
                RoutingStrategy.Bubble,
                typeof(RoutedEventHandler),
                typeof(DataField));

        private bool textContentChanged = false;

        public class DataFieldEventArgs : RoutedEventArgs
        {
            private string _fieldData = "";
            private IDictionary<string, object> _changedValues = new Dictionary<string, object>();

            public string FieldData
            {
                get { return _fieldData; }
                set { _fieldData = value; }
            }

            public IDictionary<string, object> ChangedValuesObjects
            {
                get { return _changedValues; }
                set { _changedValues = value; }
            }

            public DataFieldEventArgs() : base()
            {

            }

            public DataFieldEventArgs(RoutedEvent routedEvent) : base(routedEvent)
            {

            }
        }

        public event RoutedEventHandler DataFieldChanged
        {
            add { AddHandler(DataFieldChangedEvent, value); }
            remove { RemoveHandler(DataFieldChangedEvent, value); }
        }

 
        private object _itemSource;
        private string _previousValue;
        private object _currentValue;
        protected string _dataField = string.Empty;
        protected string _tableName = string.Empty;
        private ComponentFiller _componentFiller = new ComponentFiller();
                
       
        /// <summary>
        /// Data Source Dependency Property
        /// </summary>
        public static DependencyProperty DataSourceDependencyProperty
            = DependencyProperty.Register(
                "DataSource",
                typeof(object),
                typeof(DataField),
                new PropertyMetadata(null, OnDataSourceChanged));
        /// <summary>
        ///  Data source property handy way to associate a data field to a value.
        /// </summary>
        public object DataSource
        {
            get { return GetValue(DataSourceDependencyProperty); }
            set { SetValue(DataSourceDependencyProperty, value); }
        }

        private static void OnDataSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataField control = d as DataField;
            if (control != null)
            {
                control.OnDataSourceChanged(e);
            }
        }
      
        private void HandleSourceAsTable(object source)
        {
            DataTable table = source as DataTable;
            if (source == null)
            {
                return;
            }
            if (!string.IsNullOrEmpty(DataSourcePath))
            {
                if (table != null)
                {
                    DataColumnCollection collection = table.Columns;
                    if (collection.Contains(DataSourcePath))
                    {
                        DataRow dataRow = table.Rows[0];
                        string value = _componentFiller.FetchDataFieldValue(table, DataSourcePath);
                        if (value.Contains("#"))
                        {
                            value = value.Replace("#", "@");
                        }
                        TextField.Text = value;
                        if (IsReadOnly)
                        {
                            TextField.Background = Brushes.CadetBlue;

                        }
                        else
                        {
                            TextField.Background = Brushes.White;
                        }
                    }
                }
            }
        }
        /// <summary>
        ///  Data source property handy way to associate a data field to a value.
        /// </summary>
        protected virtual void OnDataSourceChanged(DependencyPropertyChangedEventArgs e)
        {

            if (e.NewValue == null)
            {
                return;
            }
            if (string.IsNullOrEmpty(_dataField))
            {
                return;
            }
            if (e.NewValue is DataTable)
            {
                HandleSourceAsTable(e.NewValue);
            }
            else
            {
                // ok is a poco.
                object value = e.NewValue;
                TextContent = _componentFiller.GetTextValue(value, _dataField);
            }
        }
        
        
     
        public static readonly DependencyProperty IsReadOnlyDependencyProperty =
            DependencyProperty.Register(
                "IsReadOnly",
                typeof(bool),
                typeof(DataField),
                new PropertyMetadata(false, IsReadOnlyDependencyPropertyChange));

        private static void IsReadOnlyDependencyPropertyChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataField control = d as DataField;
            if (control != null)
            {
                control.OnIsReadOnlyChanged(e);
            }
        }

        private void OnIsReadOnlyChanged(DependencyPropertyChangedEventArgs e)
        {
            bool value = Convert.ToBoolean(e.NewValue);
            if (value)
            {
                this.TextField.IsReadOnly = value;
            }
        }

        public bool IsReadOnly
        {
            get { return (bool)GetValue(IsReadOnlyDependencyProperty); }
            set { SetValue(IsReadOnlyDependencyProperty, value); }
        }
     
        public static DependencyProperty DataFieldDependencyProperty =
            DependencyProperty.Register(
                "DataSourcePath",
                typeof(string),
                typeof(DataField),
                new PropertyMetadata(string.Empty, OnDataFieldChanged));

        public string DataSourcePath
        {
            get { return (string)GetValue(DataFieldDependencyProperty); }
            set { SetValue(DataFieldDependencyProperty, value); }
        }
        private static void OnDataFieldChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataField dataFieldValue = d as DataField;
            if (dataFieldValue != null)
            {
                dataFieldValue.OnDataFieldPropertyChanged(e);
            }
        }

        public void OnDataFieldPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == null)
            {
                return;
            }
            if (string.IsNullOrEmpty(DataSourcePath))
            {
                return;
            }
            _dataField = e.NewValue as string;
            
            if (DataSource != null)
            {
                if (DataSource is DataTable)
                {
                    HandleSourceAsTable(DataSource);
                }
                else
                {
                    // ok is a poco.
                    object value = e.NewValue;
                    TextContent = _componentFiller.GetTextValue(DataSource, DataSourcePath);
                }
            }
        }
    
        #region TextContent Property
        public static readonly DependencyProperty TextContentDependencyProperty =
            DependencyProperty.Register(
                "TextContent",
                typeof(string),
                typeof(DataField),
                new PropertyMetadata(string.Empty, OnTextContentChange));
       
        public string TextContent
        {
            get { return (string)GetValue(TextContentDependencyProperty); }
            set { SetValue(TextContentDependencyProperty, value); }
        }


        private static void OnTextContentChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataField control = d as DataField;
            if (control != null)
            {
                control.OnTextContentPropertyChanged(e);
            }
        }

        private void OnTextContentPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            string value = e.NewValue as string;
            if (value.Contains("@"))
            {
                value = value.Replace("@", "#");
            }

            this.TextField.Text = value;
            if (string.IsNullOrEmpty(DataSourcePath))
            {
                return;
            }
            if (DataSource is DataTable)
            {
                var itemSource = (DataTable)DataSource;
                _componentFiller.FillTable(TextField, DataSourcePath, ref itemSource);
                DataSource = itemSource;
            }
            else
            {
                Type type = DataSource.GetType();
                PropertyInfo info = type.GetProperty(DataSourcePath);
                object propertyValue = null;
                if (info != null)
                {
                    if (type == typeof(bool))
                    {
                        propertyValue = Convert.ToBoolean(info.GetValue(DataSource)); 
                    }
                    if (type == typeof(int))
                    {
                        propertyValue = Convert.ToInt32(info.GetValue(DataSource));
                    }
                    if (type == typeof(string))
                    {
                        propertyValue = Convert.ToString(info.GetValue(DataSource));
                    }
                   // info.SetValue(DataSource, propertyValue);
                }
            }
        }
        #endregion
        #region LabelVisible

        public static readonly DependencyProperty LabelVisibleDependencyProperty =
            DependencyProperty.Register("LabelVisible",
                typeof(bool),
                typeof(DataField),
                new PropertyMetadata(false, OnLabelVisibleChange));

        public bool LabelVisible
        {
            get { return (bool)GetValue(LabelVisibleDependencyProperty); }
            set { SetValue(LabelVisibleDependencyProperty, value); }
        }

        private static void OnLabelVisibleChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataField control = d as DataField;
            if (control != null)
            {
                control.OnLabelVisibleChanged(e);
            }
        }

        private void OnLabelVisibleChanged(DependencyPropertyChangedEventArgs e)
        {
            bool value = Convert.ToBoolean(e.NewValue);
            if (value)
            {
                this.LabelField.Visibility = Visibility.Visible;
            }
            else
            {
                this.LabelField.Visibility = Visibility.Hidden;
            }
        }
        #endregion
        #region LabelText
        public static readonly DependencyProperty LabelTextDependencyProperty =
            DependencyProperty.Register(
                "LabelText",
                typeof(string),
                typeof(DataField),
                new PropertyMetadata(string.Empty, OnLabelTextChange));

        public string LabelText
        {
            get { return (string)GetValue(LabelTextDependencyProperty); }
            set { SetValue(LabelTextDependencyProperty, value); }
        }
        private static void OnLabelTextChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataField control = d as DataField;
            if (control != null)
            {
                control.OnLabelTextChanged(e);
            }
        }
        private void OnLabelTextChanged(DependencyPropertyChangedEventArgs e)
        {
            string label = e.NewValue as string;
            this.LabelField.Text = label;
        }
        public readonly static DependencyProperty LabelTextWidthDependencyProperty =
            DependencyProperty.Register(
                "LabelTextWidth",
                typeof(string),
                typeof(DataField),
                new PropertyMetadata("70", OnLabelTextWidthChange));

        public string LabelTextWidth
        {
            get { return (string)GetValue(LabelTextWidthDependencyProperty); }
            set { SetValue(LabelTextWidthDependencyProperty, value); }
        }
        private static void OnLabelTextWidthChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataField control = d as DataField;
            if (control != null)
            {
                control.OnLabelTextWidthChanged(e);
            }
        }

        private void OnLabelTextWidthChanged(DependencyPropertyChangedEventArgs e)
        {
            double value = Convert.ToDouble(e.NewValue);
            LabelField.Width = value;
        }

        #endregion
      
        #region TextContentWidth

        public string TextContentWidth
        {
            get { return (string)GetValue(TextContentWidthDependencyProperty); }
            set {  SetValue(TextContentWidthDependencyProperty, value); }
        }

        public readonly static DependencyProperty TextContentWidthDependencyProperty =
            DependencyProperty.Register(
                "TextContentWidth",
                typeof(string),
                typeof(DataField), new PropertyMetadata("200", OnTextContentWidthChange));

        private static void OnTextContentWidthChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataField control = d as DataField;
            if (control != null)
            {
                control.OnTextContentWidthPropertyChanged(e);
            }
        }
        private void OnTextContentWidthPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            string tmpValue = e.NewValue as string;
            double valueName = Convert.ToDouble(tmpValue);
            TextField.Width = valueName;
        }
        #endregion


        #region DataFieldHeight 
        public readonly static DependencyProperty DataFieldHeightDependencyProperty =
            DependencyProperty.Register(
                "DataFieldHeight",
                typeof(string),
                typeof(DataField),
                new PropertyMetadata("40", OnDataFieldHeightChange));

        public string DataFieldHeight
        {
            get { return (string)GetValue(DataFieldHeightDependencyProperty); }
            set { SetValue(DataFieldHeightDependencyProperty, value); }
        }
        private static void OnDataFieldHeightChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataField control = d as DataField;
            if (control != null)
            {
                control.OnDataFieldHeightChanged(e);
            }
        }

        private void OnDataFieldHeightChanged(DependencyPropertyChangedEventArgs e)
        {
            double value = Convert.ToDouble(e.NewValue);
            LabelField.Height = value;
            TextField.Height = value;
        }

        #endregion


        public static string CHANGED_VALUE = "ChangedValue";
        public static string FIELD = "Field";
        public static string DATATABLE = "DataTable";
        public static string TABLENAME = "TableName";

        public DataField()
        {
            InitializeComponent();
            LabelVisible = true;
            TextField.IsReadOnly = false;
            TextField.TextChanged += TextField_TextChanged;
            TextField.GotFocus += TextField_GotFocus;
            // safe defaults for the designer
            LabelField.Width = 70;
            LabelTextWidth = "70";
            TextField.Width = 150;
            LabelField.Height = 40;
            TextField.Height = 40;
            TextField.Visibility = Visibility.Visible;
            this.GotFocus += DataField_GotFocus;
            this.LostFocus += DataField_LostFocus;
            this.DataFieldContent.DataContext = this;
        }

        private void TextField_TextChanged(object sender, TextChangedEventArgs e)
        {
            textContentChanged = true;
            RaiseEvent(e);
        }

        private void DataField_LostFocus(object sender, RoutedEventArgs e)
        {
                if ((TextField.Text.Length > 0) && (textContentChanged))
            {
                    DataFieldEventArgs ev = new DataFieldEventArgs(DataFieldChangedEvent);
                    ev.FieldData = TextField.Text;
                    IDictionary<string, object> valueDictionary = new Dictionary<string, object>();
                    valueDictionary["Field"] = DataSourcePath;
                    valueDictionary["DataObject"] = DataSource;
                    valueDictionary["ChangedValue"] = TextField.Text;
                    valueDictionary["PreviousValue"] = _previousValue;
                    ev.ChangedValuesObjects = valueDictionary;
                    RaiseEvent(ev);
                }
                textContentChanged = false;
        }

        private void TextField_GotFocus(object sender, RoutedEventArgs e)
        {
            textContentChanged = false;
            this.TextField.SelectAll();
            RaiseEvent(e);
        }

        private void DataField_GotFocus(object sender, RoutedEventArgs e)
        {
            TextField.Focus();
        }
    }
}