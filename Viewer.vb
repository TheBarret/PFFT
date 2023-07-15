Imports System.Net.NetworkInformation

Public Class Viewer
    Public Property Hostname As String
    Public Property Samplerate As Integer
    Public Property Buffer As List(Of Double)

    Private Sub Viewer_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Hostname = "www.google.nl"
        Me.Samplerate = 512
        Me.Buffer = New List(Of Double)
        Me.Prepare()
        Me.Timer.Enabled = True
        Me.Timer.Interval = 1000
        Me.Timer.Start()
    End Sub

    Private Sub Prepare()
        For i As Integer = 0 To Me.Samplerate - 1
            Me.Buffer.Add(0F)
        Next
    End Sub

    Private Sub Timer_Tick(sender As Object, e As EventArgs) Handles Timer.Tick
        SyncLock Me.Buffer
            Me.Buffer.Add(Me.Send(Me.Hostname) * 1.0R)
            If (Me.Buffer.Count > Me.Samplerate) Then
                Me.Buffer.RemoveAt(0)
            End If
        End SyncLock
        Me.Snapshot()
    End Sub

    Public Function Send(host As String) As Long
        Try
            Using icmp As New Ping
                Dim reply As PingReply = icmp.Send(host, 5000, New Byte(0) {0}, New PingOptions With {.DontFragment = True})
                If reply.Status = IPStatus.Success Then
                    Return reply.RoundtripTime
                Else
                    Return -1
                End If
            End Using
        Catch
            Return -1
        End Try
    End Function

    Public Sub Snapshot()
        Dim data As Complex() = Me.ConvertToComplex(Me.Buffer.ToArray)
        Dim conv As Complex() = Me.Transform(data)
        Me.PPBlack(conv)
        Dim params = Me.GetMaxFreq(conv, Me.Samplerate)
        Me.Label1.Text = String.Format("Max Frequency: {0:F2}Hz", params.Item1)
        Me.Label2.Text = String.Format("Min Frequency: {0:F2}Hz", params.Item2)
        Me.Label3.Text = String.Format("Avg Frequency: {0:F2}Hz", params.Item3)
        Me.Vp.Render(conv, 0.005)
    End Sub

    ' Complex Convertor
    Private Function ConvertToComplex(buffer As Double()) As Complex()
        Dim signal As Complex() = New Complex(buffer.Length - 1) {}
        For i As Integer = 0 To buffer.Length - 1
            signal(i) = New Complex(buffer(i), 0)
        Next
        Return signal
    End Function

    ' FFT
    Private Function Transform(frame As Complex()) As Complex()
        Dim N As Integer = frame.Length
        ' Return frame if length = 1
        If N = 1 Then Return frame
        ' Split signal into even and odd-indexed elements
        Dim even(N \ 2 - 1) As Complex
        Dim odd(N \ 2 - 1) As Complex
        For i As Integer = 0 To N - 1 Step 2
            even(i \ 2) = frame(i)
            odd(i \ 2) = frame(i + 1)
        Next
        ' Recursive FFT calls
        Dim evenResult As Complex() = Transform(even)
        Dim oddResult As Complex() = Transform(odd)
        ' Combine results
        Dim combined(N - 1) As Complex
        For k As Integer = 0 To N \ 2 - 1
            Dim factor As Complex = New Complex(Math.Cos(-2 * Math.PI * k / N), Math.Sin(-2 * Math.PI * k / N))
            combined(k) = evenResult(k) + factor * oddResult(k)
            combined(k + N \ 2) = evenResult(k) - factor * oddResult(k)
        Next
        Return combined
    End Function

    ' Postprocessor
    Private Sub PPHam(ByRef signal() As Complex)
        For i As Integer = 0 To signal.Length - 1
            signal(i).Real = CSng(signal(i).Real * (0.54 - 0.46 * Math.Cos(2 * Math.PI * i / (signal.Length - 1))))
        Next
    End Sub

    ' Postprocessor
    Private Sub PPBlack(ByRef signal() As Complex)
        For i As Integer = 0 To signal.Length - 1
            signal(i).Real = CSng(signal(i).Real * (1 - 0.08 * Math.Cos(2 * Math.PI * i / (signal.Length - 1))))
        Next
    End Sub

    Private Function GetMaxFreq(data As Complex(), samplerate As Integer) As Tuple(Of Double, Double, Double)
        Dim maxMagnitude As Double = Double.MinValue
        Dim minMagnitude As Double = Double.MaxValue
        Dim maxFreq As Double = 0.0
        Dim minFreq As Double = 0.0
        Dim totalMagnitude As Double = 0.0
        Dim totalCount As Integer = 0

        For i As Integer = 0 To data.Length - 1
            Dim magnitude As Double = data(i).Magnitude()
            Dim frequency As Double = (i / data.Length) * (samplerate / 2) ' in Hz

            If magnitude > maxMagnitude Then
                maxMagnitude = magnitude
                maxFreq = frequency
            End If
            If magnitude < minMagnitude Then
                minMagnitude = magnitude
                minFreq = frequency
            End If

            totalMagnitude += magnitude
            totalCount += 1
        Next

        Dim averageMagnitude As Double = totalMagnitude / totalCount
        ' maxFreq is the frequency with the highest magnitude,
        ' minFreq is the frequency with the lowest magnitude
        ' averageMagnitude is the average magnitude across all frequencies.
        Return New Tuple(Of Double, Double, Double)(maxFreq, minFreq, averageMagnitude)
    End Function

    Public Shared Function Range(min As Single, max As Single) As Single
        Return CSng((max - min) * Viewer.Randomizer.NextDouble + min)
    End Function

    Public Shared ReadOnly Property Randomizer As Random
        Get
            Static r As New Random(DateTime.Now.Millisecond)
            Return r
        End Get
    End Property

End Class
