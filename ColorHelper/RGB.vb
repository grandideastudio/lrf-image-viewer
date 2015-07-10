''' <summary>
''' Structure to define RGB.
''' </summary>
Public Structure RGB
#Region "Fields"

    Private _Red As Integer
    Private _Green As Integer
    Private _Blue As Integer

#End Region

#Region "Operators"

    Public Shared Operator =(ByVal item1 As RGB, ByVal item2 As RGB) As Boolean
        Return item1.Red = item2.Red AndAlso _
               item1.Green = item2.Green AndAlso _
               item1.Blue = item2.Blue
    End Operator

    Public Shared Operator <>(ByVal item1 As RGB, ByVal item2 As RGB) As Boolean
        Return item1.Red <> item2.Red OrElse _
               item1.Green <> item2.Green OrElse _
               item1.Blue <> item2.Blue
    End Operator

#End Region

#Region "Accessors"

    Public Property Red()
        Get
            Return _Red
        End Get
        Set(ByVal value)
            If value > 255 Then
                _Red = 255
            Else
                If value < 0 Then
                    _Red = 0
                Else
                    _Red = value
                End If
            End If
        End Set
    End Property

    Public Property Green()
        Get
            Return _Green
        End Get
        Set(ByVal value)
            If value > 255 Then
                _Green = 255
            Else
                If value < 0 Then
                    _Green = 0
                Else
                    _Green = value
                End If
            End If
        End Set
    End Property

    Public Property Blue()
        Get
            Return _Blue
        End Get
        Set(ByVal value)
            If value > 255 Then
                _Blue = 255
            Else
                If value < 0 Then
                    _Blue = 0
                Else
                    _Blue = value
                End If
            End If
        End Set
    End Property

#End Region

    ''' <summary>
    ''' Creates an instance of a RGB structure.
    ''' </summary>
    ''' <param name="r">Red value (must be between 0 and 255).</param>
    ''' <param name="g">Green value (must be between 0 and 255).</param>
    ''' <param name="b">Blue value (must be between 0 and 255).</param>
    Public Sub New(ByVal r As Integer, ByVal g As Integer, ByVal b As Integer)
        Me.Red = r
        Me.Green = g
        Me.Blue = b
    End Sub

#Region "Methods"

    Public Overrides Function Equals(ByVal obj As Object) As Boolean
        If (obj Is Nothing) Or (Me.GetType() IsNot obj.GetType()) Then Return False
        Return (Me = CType(obj, RGB))
    End Function

    Public Overrides Function GetHashCode() As Integer
        Return Red.GetHashCode() ^ Green.GetHashCode() ^ Blue.GetHashCode()
    End Function

#End Region

End Structure