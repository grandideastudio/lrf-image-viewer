''' <summary>
''' Structure to define CIELab.
''' </summary>
Public Structure CIELab
#Region "Fields"

    Private _l As Integer
    Private _a As Integer
    Private _b As Integer

#End Region

#Region "Operators"

    Public Shared Operator =(ByVal item1 As CIELab, ByVal item2 As CIELab) As Boolean
        Return item1.L = item2.L AndAlso _
               item1.A = item2.A AndAlso _
               item1.B = item2.B
    End Operator

    Public Shared Operator <>(ByVal item1 As CIELab, ByVal item2 As CIELab) As Boolean
        Return item1.L <> item2.A OrElse _
               item1.A <> item2.A OrElse _
               item1.B <> item2.B
    End Operator

#End Region

#Region "Accessors"

    Public Property L()
        Get
            Return _l
        End Get
        Set(ByVal value)
           _l = value
        End Set
    End Property

    Public Property A()
        Get
            Return _a
        End Get
        Set(ByVal value)
            _a = value
        End Set
    End Property

    Public Property B()
        Get
            Return _b
        End Get
        Set(ByVal value)
            _b = value
        End Set
    End Property

#End Region

    ''' <summary>
    ''' Creates an instance of a CIELAB structure.
    ''' </summary>
    Public Sub New(ByVal l As Double, ByVal a As Double, ByVal b As Double)
        Me.L = l
        Me.A = A
        Me.B = b
    End Sub

#Region "Methods"

    Public Overrides Function Equals(ByVal obj As Object) As Boolean
        If (obj Is Nothing) Or (Me.GetType() IsNot obj.GetType()) Then Return False
        Return (Me = CType(obj, CIELab))
    End Function

    Public Overrides Function GetHashCode() As Integer
        Return L.GetHashCode() ^ A.GetHashCode() ^ B.GetHashCode()
    End Function

#End Region

End Structure