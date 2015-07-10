''' <summary>
''' Structure to define HSL.
''' </summary>
Public Structure HSL

#Region "Fields"

    Private _Hue As Double
    Private _Saturation As Double
    Private _Luminance As Double

#End Region

#Region "Operators"

    Public Shared Operator =(ByVal item1 As HSL, ByVal item2 As HSL) As Boolean
        Return item1.Hue = item2.Hue AndAlso _
               item1.Saturation = item2.Saturation AndAlso _
               item1.Luminance = item2.Luminance
    End Operator

    Public Shared Operator <>(ByVal item1 As HSL, ByVal item2 As HSL) As Boolean
        Return item1.Hue <> item2.Hue OrElse _
               item1.Saturation <> item2.Saturation OrElse _
               item1.Luminance <> item2.Luminance
    End Operator

#End Region

#Region "Accessors"

    Public Property Hue()
        Get
            Return _Hue
        End Get
        Set(ByVal value)
            If value > 360 Then
                _Hue = 360
            Else
                If value < 0 Then
                    _Hue = 0
                Else
                    _Hue = value
                End If
            End If
        End Set
    End Property

    Public Property Saturation()
        Get
            Return _Saturation
        End Get
        Set(ByVal value)
            If value > 1 Then
                _Saturation = 1
            Else
                If value < 0 Then
                    _Saturation = 0
                Else
                    _Saturation = value
                End If
            End If
        End Set
    End Property

    Public Property Luminance()
        Get
            Return _Luminance
        End Get
        Set(ByVal value)
            If value > 1 Then
                _Luminance = 1
            Else
                If value < 0 Then
                    _Luminance = 0
                Else
                    _Luminance = value
                End If
            End If
        End Set
    End Property

#End Region

    ''' <summary>
    ''' Creates an instance of a HSL structure.
    ''' </summary>
    ''' <param name="h">Hue value (must be between 0 and 360).</param>
    ''' <param name="s">Saturation value (must be between 0 and 1).</param>
    ''' <param name="l">Luminance value (must be between 0 and 1).</param>
    Public Sub New(ByVal h As Double, ByVal s As Double, ByVal l As Double)
        Me.Hue = h
        Me.Saturation = s
        Me.Luminance = l
    End Sub

#Region "Methods"

    Public Overrides Function Equals(ByVal obj As Object) As Boolean
        If (obj Is Nothing) Or (Me.GetType() IsNot obj.GetType()) Then Return False
        Return (Me = CType(obj, HSL))
    End Function

    Public Overrides Function GetHashCode() As Integer
        Return Hue.GetHashCode() ^ Saturation.GetHashCode() ^ Luminance.GetHashCode()
    End Function

#End Region

End Structure