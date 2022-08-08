Imports DevExpress.Xpf.Core
Imports System
Imports System.Collections.Generic
Imports System.IO
Imports System.Windows
Imports System.Windows.Resources
Imports System.Xml.Linq

Namespace ChartFiltering

    Public Partial Class MainWindow
        Inherits DevExpress.Xpf.Core.ThemedWindow

        Public Sub New()
            Me.InitializeComponent()
            Dim dataStreamInfo As System.Windows.Resources.StreamResourceInfo = System.Windows.Application.GetResourceStream(New System.Uri("Data/GDPStatistic.xml", System.UriKind.RelativeOrAbsolute))
            Dim valueProvider As ChartFiltering.XmlGdpValueProvider = New ChartFiltering.XmlGdpValueProvider(dataStreamInfo.Stream)
            Me.DataContext = New ChartFiltering.MainViewModel(valueProvider)
        End Sub
    End Class

    Public Class MainViewModel

        Private _GdpValues As IEnumerable(Of ChartFiltering.GdpValue)

        Public Property GdpValues As IEnumerable(Of ChartFiltering.GdpValue)
            Get
                Return _GdpValues
            End Get

            Private Set(ByVal value As IEnumerable(Of ChartFiltering.GdpValue))
                _GdpValues = value
            End Set
        End Property

        Public Sub New(ByVal valueProvider As ChartFiltering.XmlGdpValueProvider)
            Me.GdpValues = valueProvider.GetValues()
        End Sub
    End Class

    Public Class XmlGdpValueProvider

        Private xmlStream As System.IO.Stream

        Public Sub New(ByVal xmlStream As System.IO.Stream)
            Me.xmlStream = xmlStream
        End Sub

        Public Function GetValues() As IEnumerable(Of ChartFiltering.GdpValue)
            Dim dataSet As System.Xml.Linq.XElement = System.Xml.Linq.XDocument.Load(CType((Me.xmlStream), System.IO.Stream)).Element("data-set")
            Dim values As System.Collections.Generic.List(Of ChartFiltering.GdpValue) = New System.Collections.Generic.List(Of ChartFiltering.GdpValue)()
            For Each continent As System.Xml.Linq.XElement In dataSet.Elements("ContinentInfo")
                Dim continentName As System.[String] = continent.Element(CType(("ContinentName"), System.Xml.Linq.XName)).Value
                For Each country As System.Xml.Linq.XElement In continent.Element(CType(("Countries"), System.Xml.Linq.XName)).Elements("CountryInfo")
                    Dim countryName As System.[String] = country.Element(CType(("Name"), System.Xml.Linq.XName)).Value
                    For Each gdpByYear As System.Xml.Linq.XElement In country.Element(CType(("Statistic"), System.Xml.Linq.XName)).Elements("GDPByYear")
                        Dim year As Integer = Integer.Parse(gdpByYear.Element(CType(("Year"), System.Xml.Linq.XName)).Value)
                        Dim value As Double = If(System.[String].IsNullOrEmpty(gdpByYear.Element(CType(("GDP"), System.Xml.Linq.XName)).Value), 0, Double.Parse(gdpByYear.Element(CType(("GDP"), System.Xml.Linq.XName)).Value))
                        values.Add(New ChartFiltering.GdpValue With {.ContinentName = continentName, .CountryName = countryName, .Year = year, .Value = value / 1000000000000.0})
                    Next
                Next
            Next

            Return values
        End Function
    End Class

    Public Class GdpValue

        Public Property ContinentName As String

        Public Property CountryName As String

        Public Property Value As Double

        Public Property Year As Integer
    End Class
End Namespace
