''' <summary>
''' Structure to define CMYK.
''' </summary>
Public Structure CMYK

#Region "Fields"

    Private _Cyan As Double
    Private _Magenta As Double
    Private _Yellow As Double
    Private _Black As Double

#End Region

#Region "Operators"

    Public Shared Operator =(ByVal item1 As CMYK, ByVal item2 As CMYK) As Boolean
        Return item1.Cyan = item2.Cyan AndAlso _
               item1.Magenta = item2.Magenta AndAlso _
               item1.Yellow = item2.Yellow AndAlso _
               item1.Black = item2.Black
    End Operator

    Public Shared Operator <>(ByVal item1 As CMYK, ByVal item2 As CMYK) As Boolean
        Return item1.Cyan <> item2.Cyan OrElse _
               item1.Magenta <> item2.Magenta OrElse _
               item1.Yellow <> item2.Yellow OrElse _
               item1.Black <> item2.Black
    End Operator

#End Region

#Region "Accessors"

    Public Property Cyan()
        Get
            Return _Cyan
        End Get
        Set(ByVal value)
            If value > 1 Then
                _Cyan = 1
            Else
                If value < 0 Then
                    _Cyan = 0
                Else
                    _Cyan = value
                End If
            End If
        End Set
    End Property

    Public Property Magenta()
        Get
            Return _Magenta
        End Get
        Set(ByVal value)
            If value > 1 Then
                _Magenta = 1
            Else
                If value < 0 Then
                    _Magenta = 0
                Else
                    _Magenta = value
                End If
            End If
        End Set
    End Property

    Public Property Yellow()
        Get
            Return _Yellow
        End Get
        Set(ByVal value)
            If value > 1 Then
                _Yellow = 1
            Else
                If value < 0 Then
                    _Yellow = 0
                Else
                    _Yellow = value
                End If
            End If
        End Set
    End Property

    Public Property Black()
        Get
            Return _Black
        End Get
        Set(ByVal value)
            If value > 1 Then
                _Black = 1
            Else
                If value < 0 Then
                    _Black = 0
                Else
                    _Black = value
                End If
            End If
        End Set
    End Property

#End Region

    ''' <summary>
    ''' Creates an instance of a CMYK structure.
    ''' </summary>
    ''' <param name="c">Cyan value (must be between 0 and 1).</param>
    ''' <param name="m">Magenta value (must be between 0 and 1).</param>
    ''' <param name="y">Yellow value (must be between 0 and 1).</param>
    ''' <param name="k">Black value (must be between 0 and 1).</param>
    Public Sub New(ByVal c As Double, ByVal m As Double, ByVal y As Double, ByVal k As Double)
        Me.Cyan = c
        Me.Magenta = m
        Me.Yellow = y
        Me.Black = k
    End Sub

#Region "Methods"

    Public Overrides Function Equals(ByVal obj As Object) As Boolean
        If (obj Is Nothing) Or (Me.GetType() IsNot obj.GetType()) Then Return False
        Return (Me = CType(obj, CMYK))
    End Function

    Public Overrides Function GetHashCode() As Integer
        Return Cyan.GetHashCode() ^ Magenta.GetHashCode() ^ Yellow.GetHashCode() ^ Black.GetHashCode()
    End Function

#End Region

End Structure