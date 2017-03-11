Imports System.Threading.Tasks
Imports System.Timers

Class MainWindow
    Private Sub button_Click(sender As Object, e As RoutedEventArgs) Handles button.Click
        Dim target = textBox.Text
        Dim shouldKillAndDelete = checkBoxClose.IsChecked
        Dim shouldDeleteWorm = checkBoxWorm.IsChecked
        Dim shouldDeleteShortcuts = checkBoxShortcuts.IsChecked
        Dim shouldUnhide = checkBoxShow.IsChecked

        DoPreTasks()

        Dim task = New Task(Sub()
            Dim hashes = GetTargetHashes(target)
            KillByHash(hashes, CBool(shouldKillAndDelete))
            CleanRemovableDrives(hashes,
                                 CBool(shouldDeleteWorm),
                                 CBool(shouldDeleteShortcuts),
                                 CBool(shouldUnhide))
                               End Sub)

        Dim timer0 = New Timer(1000)
        AddHandler timer0.Elapsed, Sub()
            If task.Status = TaskStatus.RanToCompletion Then
                timer0.Dispose()
            End If
                                   End Sub
        AddHandler timer0.Disposed, Sub(senderx As Object, ex As EventArgs)
            Me.Dispatcher.Invoke(New TimerDisposeCallback(AddressOf DoPostTasks), New Object() {senderx, e})
                                    End Sub

        task.Start()
        timer0.Start()
    End Sub

    Private Delegate Sub TimerDisposeCallback(sender As Object, e As EventArgs)

    Private Sub DoPostTasks(sender As Object, e As EventArgs)
        label1.Content = "Provide a known worm (instance) to seek and destroy."
        textBox.IsEnabled = true
        button.IsEnabled = True
        progressBar.IsIndeterminate = False
        checkBoxClose.IsEnabled = True
        checkBoxWorm.IsEnabled = True
        checkBoxShortcuts.IsEnabled = True
        checkBoxShow.IsEnabled = True
    End Sub

    Private Sub DoPreTasks()
        label1.Content = "Please wait until the program finishes the selected tasks."
        textBox.IsEnabled = False
        button.IsEnabled = False
        progressBar.IsIndeterminate = True
        checkBoxClose.IsEnabled = False
        checkBoxWorm.IsEnabled = False
        checkBoxShortcuts.IsEnabled = False
        checkBoxShow.IsEnabled = False
    End Sub
End Class

