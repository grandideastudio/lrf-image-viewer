''' <summary>
''' Structure to define YUV.
''' </summary>
Public Structure YUV

#Region "Fields"

    Private _Y As Double
    Private _U As Double
    Private _V As Double

#End Region

#Region "Operators"

    Public Shared Operator =(ByVal item1 As YUV, ByVal item2 As YUV) As Boolean
        Return item1.Y = item2.Y AndAlso _
               item1.U = item2.U AndAlso _
               item1.V = item2.V
    End Operator

    Public Shared Operator <>(ByVal item1 As YUV, ByVal item2 As YUV) As Boolean
        Return item1.Y <> item2.Y OrElse _
               item1.U <> item2.U OrElse _
               item1.V <> item2.V
    End Operator

    ' Added by J. Grand, 3/30/11
    Public Shared Operator <=(ByVal item1 As YUV, ByVal item2 As YUV) As Boolean
        Return item1.Y <= item2.Y AndAlso _
               item1.U <= item2.U AndAlso _
               item1.V <= item2.V
    End Operator

    Public Shared Operator >=(ByVal item1 As YUV, ByVal item2 As YUV) As Boolean
        Return item1.Y >= item2.Y AndAlso _
               item1.U >= item2.U AndAlso _
               item1.V >= item2.V
    End Operator


#End Region

#Region "Accessors"

    Public Property Y()
        Get
            Return _Y
        End Get
        Set(ByVal value)
            If value > 1 Then
                _Y = 1
            Else
                If value < 0 Then
                    _Y = 0
                Else
                    _Y = value
                End If
            End If
        End Set
    End Property

    Public Property U()
        Get
            Return _U
        End Get
        Set(ByVal value)
            If value > 0.436 Then
                _U = 0.436
            Else
                If value < -0.436 Then
                    _U = -0.436
                Else
                    _U = value
                End If
            End If
        End Set
    End Property

    Public Property V()
        Get
            Return _V
        End Get
        Set(ByVal value)
            If value > 0.615 Then
                _V = 0.615
            Else
                If value < -0.615 Then
                    _V = -0.615
                Else
                    _V = value
                End If
            End If
        End Set
    End Property

#End Region

    ''' <summary>
    ''' Creates an instance of a YUV structure.
    ''' </summary>
    ''' <param name="y">Y (must be between 0 and 1).</param>
    ''' <param name="u">U (must be between -0.436 and 0.436).</param>
    ''' <param name="v">V (must be between -0.436 and 0.436).</param>
    Public Sub New(ByVal y As Double, ByVal u As Double, ByVal v As Double)
        Me.Y = y
        Me.U = u
        Me.V = v
    End Sub

#Region "Methods"

    Public Overrides Function Equals(ByVal obj As Object) As Boolean
        If (obj Is Nothing) Or (Me.GetType() IsNot obj.GetType()) Then Return False
        Return (Me = CType(obj, YUV))
    End Function

    Public Overrides Function GetHashCode() As Integer
        Return Y.GetHashCode() ^ U.GetHashCode() ^ V.GetHashCode()
    End Function

#End Region

End Structure