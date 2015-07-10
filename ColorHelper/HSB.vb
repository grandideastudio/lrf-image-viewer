''' <summary>
''' Structure to define HSB.
''' </summary>
Public Structure HSB

#Region "Fields"

    Private _Hue As Double
    Private _Saturation As Double
    Private _Brightness As Double

#End Region

#Region "Operators"

    Public Shared Operator =(ByVal item1 As HSB, ByVal item2 As HSB) As Boolean
        Return item1.Hue = item2.Hue AndAlso _
               item1.Saturation = item2.Saturation AndAlso _
               item1.Brightness = item2.Brightness
    End Operator

    Public Shared Operator <>(ByVal item1 As HSB, ByVal item2 As HSB) As Boolean
        Return item1.Hue <> item2.Hue OrElse _
               item1.Saturation <> item2.Saturation OrElse _
               item1.Brightness <> item2.Brightness
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

    Public Property Brightness()
        Get
            Return _Brightness
        End Get
        Set(ByVal value)
            If value > 1 Then
                _Brightness = 1
            Else
                If value < 0 Then
                    _Brightness = 0
                Else
                    _Brightness = value
                End If
            End If
        End Set
    End Property

#End Region

    ''' <summary>
    ''' Creates an instance of a HSB structure.
    ''' </summary>
    ''' <param name="h">Hue value (must be between 0 and 360).</param>
    ''' <param name="s">Saturation value (must be between 0 and 1).</param>
    ''' <param name="b">Brightness value (must be between 0 and 1).</param>
    Public Sub New(ByVal h As Double, ByVal s As Double, ByVal b As Double)
        Me.Hue = h
        Me.Saturation = s
        Me.Brightness = b
    End Sub

#Region "Methods"

    Public Overrides Function Equals(ByVal obj As Object) As Boolean
        If (obj Is Nothing) Or (Me.GetType() IsNot obj.GetType()) Then Return False
        Return (Me = CType(obj, HSB))
    End Function

    Public Overrides Function GetHashCode() As Integer
        Return Hue.GetHashCode() ^ Saturation.GetHashCode() ^ Brightness.GetHashCode()
    End Function

#End Region

End Structure