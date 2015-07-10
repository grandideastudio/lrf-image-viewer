''' <summary>
''' Structure to define CIEXYZ.
''' </summary>
Public Structure CIEXYZ

#Region "Fields"

    Private _x As Double
    Private _y As Double
    Private _z As Double

#End Region

#Region "Operators"

    Public Shared Operator =(ByVal item1 As CIEXYZ, ByVal item2 As CIEXYZ) As Boolean
        Return item1.X = item2.X AndAlso _
               item1.Y = item2.Y AndAlso _
               item1.Z = item2.Z
    End Operator

    Public Shared Operator <>(ByVal item1 As CIEXYZ, ByVal item2 As CIEXYZ) As Boolean
        Return item1.X <> item2.X OrElse _
               item1.Y <> item2.Y OrElse _
               item1.Z <> item2.Z
    End Operator

#End Region

#Region "Accessors"

    Public Property X()
        Get
            Return _x
        End Get
        Set(ByVal value)
            If value > 0.9505 Then
                _x = 0.9505
            Else
                If value < 0 Then
                    _x = 0
                Else
                    _x = value
                End If
            End If
        End Set
    End Property

    Public Property Y()
        Get
            Return _y
        End Get
        Set(ByVal value)
            If value > 1.0 Then
                _y = 1.0
            Else
                If value < 0 Then
                    _y = 0
                Else
                    _y = value
                End If
            End If
        End Set
    End Property

    Public Property Z()
        Get
            Return _z
        End Get
        Set(ByVal value)
            If value > 1.089 Then
                _z = 1.089
            Else
                If value < 0 Then
                    _z = 0
                Else
                    _z = value
                End If
            End If
        End Set
    End Property

#End Region

    ''' <summary>
    ''' Creates an instance of a CIEXYZ structure.
    ''' </summary>
    Public Sub New(ByVal x As Double, ByVal y As Double, ByVal z As Double)
        Me.X = x
        Me.Y = y
        Me.Z = z
    End Sub

#Region "Methods"

    Public Overrides Function Equals(ByVal obj As Object) As Boolean
        If (obj Is Nothing) Or (Me.GetType() IsNot obj.GetType()) Then Return False
        Return (Me = CType(obj, CIEXYZ))
    End Function

    Public Overrides Function GetHashCode() As Integer
        Return X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode()
    End Function

#End Region

End Structure