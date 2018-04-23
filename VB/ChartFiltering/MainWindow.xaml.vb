Imports System
Imports System.Collections.Generic
Imports System.IO
Imports System.Linq
Imports System.Windows
Imports System.Windows.Resources
Imports System.Xml.Linq
Imports DevExpress.Utils.Filtering
Imports DevExpress.Xpf.Core

Namespace ChartFiltering
    Partial Public Class MainWindow
        Inherits ThemedWindow

        #Region "#MainWindow_Ctor"
        Public Sub New()
            InitializeComponent()

            Dim dataStreamInfo As StreamResourceInfo = Application.GetResourceStream(New Uri("Data/GDPStatistic.xml", UriKind.RelativeOrAbsolute))
            Dim valueProvider As New XmlGdpValueProvider(dataStreamInfo.Stream)
            DataContext = New MainViewModel(valueProvider)
        End Sub
        #End Region ' #MainWindow_Ctor
    End Class

    #Region "#MainViewModel"
    Public Class MainViewModel
        Private privateGdpValues As IEnumerable(Of GdpValue)
        Public Property GdpValues() As IEnumerable(Of GdpValue)
            Get
                Return privateGdpValues
            End Get
            Private Set(ByVal value As IEnumerable(Of GdpValue))
                privateGdpValues = value
            End Set
        End Property

        'Filter parameters
        Private privateMinGdpValue As Double
        Public Property MinGdpValue() As Double
            Get
                Return privateMinGdpValue
            End Get
            Private Set(ByVal value As Double)
                privateMinGdpValue = value
            End Set
        End Property
        Private privateMaxGdpValue As Double
        Public Property MaxGdpValue() As Double
            Get
                Return privateMaxGdpValue
            End Get
            Private Set(ByVal value As Double)
                privateMaxGdpValue = value
            End Set
        End Property
        Private privateStartYear As Integer
        Public Property StartYear() As Integer
            Get
                Return privateStartYear
            End Get
            Private Set(ByVal value As Integer)
                privateStartYear = value
            End Set
        End Property
        Private privateEndYear As Integer
        Public Property EndYear() As Integer
            Get
                Return privateEndYear
            End Get
            Private Set(ByVal value As Integer)
                privateEndYear = value
            End Set
        End Property
        Private privateCountryNames As IEnumerable(Of String)
        Public Property CountryNames() As IEnumerable(Of String)
            Get
                Return privateCountryNames
            End Get
            Private Set(ByVal value As IEnumerable(Of String))
                privateCountryNames = value
            End Set
        End Property
        Private privateContinentNames As IEnumerable(Of String)
        Public Property ContinentNames() As IEnumerable(Of String)
            Get
                Return privateContinentNames
            End Get
            Private Set(ByVal value As IEnumerable(Of String))
                privateContinentNames = value
            End Set
        End Property


        Public Sub New(ByVal valueProvider As XmlGdpValueProvider)
            GdpValues = valueProvider.GetValues()
            MinGdpValue = GdpValues.Min(Function(gdp) gdp.Value)
            MaxGdpValue = GdpValues.Max(Function(gdp) gdp.Value)
            StartYear = GdpValues.Min(Function(gdp) gdp.Year)
            EndYear = GdpValues.Max(Function(gdp) gdp.Year)
            CountryNames = GdpValues.Select(Function(gdp) gdp.CountryName)
            ContinentNames = GdpValues.Select(Function(gdp) gdp.ContinentName)

        End Sub
    End Class
    #End Region ' #MainViewModel

    Public Class XmlGdpValueProvider
        Private xmlStream As Stream

        Public Sub New(ByVal xmlStream As Stream)
            Me.xmlStream = xmlStream
        End Sub

        Public Function GetValues() As IEnumerable(Of GdpValue)
            Dim dataSet As XElement = XDocument.Load(xmlStream).Element("data-set")
            Dim values As New List(Of GdpValue)()
            For Each continent As XElement In dataSet.Elements("ContinentInfo")
                Dim continentName As String = continent.Element("ContinentName").Value
                For Each country As XElement In continent.Element("Countries").Elements("CountryInfo")
                    Dim countryName As String = country.Element("Name").Value
                    For Each gdpByYear As XElement In country.Element("Statistic").Elements("GDPByYear")
                        Dim year As Integer = Integer.Parse(gdpByYear.Element("Year").Value)
                        Dim value As Double = If(String.IsNullOrEmpty(gdpByYear.Element("GDP").Value), 0, Double.Parse(gdpByYear.Element("GDP").Value))
                        values.Add(New GdpValue With { _
                            .ContinentName = continentName, _
                            .CountryName = countryName, _
                            .Year = year, _
                            .Value = value / 1000000000000.0 _
                        })
                    Next gdpByYear
                Next country
            Next continent

            Return values
        End Function
    End Class

    #Region "#GdpValue"
    Public Class GdpValue
        <FilterLookup(DataSourceMember := "ContinentNames", UseBlanks := False)> _
        Public Property ContinentName() As String

        <FilterLookup(DataSourceMember := "CountryNames", UseBlanks := False)> _
        Public Property CountryName() As String

        <FilterRange(MaximumMember := "MaxGdpValue", MinimumMember := "MinGdpValue")> _
        Public Property Value() As Double

        <FilterRange(MinimumMember := "StartYear", MaximumMember := "EndYear")> _
        Public Property Year() As Integer
    End Class
    #End Region ' #GdpValue
End Namespace
