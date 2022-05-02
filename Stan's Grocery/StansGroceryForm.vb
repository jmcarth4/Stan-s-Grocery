'Jessica McArthur
'RCET0265
'Spring 2022
'Stan's Grocery
'https://github.com/jmcarth4/Stan-s-Grocery.git

Option Strict On
Option Explicit On
Option Compare Text

Public Class StansGroceryForm
    Dim food(255, 2) As String

    'Reads information from the data file.
    Sub LoadDataFile()
        Dim filename As String = "C:\Users\jekam\Documents\RoboticsClasses\VisualBasics_Spring2022\HW\Stan's Grocery\grocery.txt"
        Dim record As String
        Dim row As Integer
        Dim temp() As String

        FileOpen(1, filename, OpenMode.Input)

        Do Until EOF(1)

            'grad item name
            Input(1, record)
            temp = Split(record, "$$ITM")
            food(row, 0) = temp(1)

            'grab location
            Input(1, record)
            temp = Split(record, "##LOC")

            Try
                food(row, 1) = temp(1)
            Catch ex As Exception
                'add empty string if error
                food(row, 1) = ""
            End Try


            'grab category
            Input(1, record)
            temp = Split(record, "%%CAT")
            Try
                food(row, 2) = temp(1)
            Catch ex As Exception
                'add empty string if error
                food(row, 2) = ""
            End Try

            row += 1
        Loop

        FileClose(1)
    End Sub

    'Displays all item names from the file in the displaylistbox. 
    Sub DisplayAllItems()
        DisplayListBox.Items.Clear()
        For i = Me.food.GetLowerBound(0) To Me.food.GetUpperBound(0)
            If Me.food(i, 0) <> "" Then
                DisplayListBox.Items.Add(Me.food(i, 0))
            End If
        Next
    End Sub

    'Organizes the different types of data filters to be displayed in the combo box. 
    Sub ComboBoxDisplay()
        Dim aisle As Integer
        Dim category As String

        FilterComboBox.Items.Clear()
        FilterComboBox.Items.Add(" ~Show All~")

        If FilterByAisleRadioButton.Checked = True Then
            For i = Me.food.GetLowerBound(0) To Me.food.GetUpperBound(0)
                Try
                    aisle = CInt(food(i, 1))
                Catch ex As Exception
                    'Catch empty aisle data
                End Try
                'Does not display duplicates
                If FilterComboBox.Items.Contains(aisle.ToString.PadLeft(2)) = False And CStr(aisle) <> "" Then
                    FilterComboBox.Items.Add(aisle.ToString.PadLeft(2))
                End If
            Next

        ElseIf FilterByCategoryRadioButton.Checked = True Then
            For i = Me.food.GetLowerBound(0) To Me.food.GetUpperBound(0)
                Try
                    category = food(i, 2)
                    'Does not display duplicates
                    If FilterComboBox.Items.Contains(category) = False And category <> "" Then
                        FilterComboBox.Items.Add(category)
                    End If
                Catch ex As Exception
                    'Catch empty category data
                End Try
            Next
        End If
        'Sorts displays of aisle and category 
        'by number order or alphabetaical order
        FilterComboBox.Sorted = True
        FilterComboBox.SelectedIndex = 0
    End Sub

    'Data is searched for the items entered into the search box. 
    Sub SearchByText()
        Dim searchText As String
        Dim blank As Boolean = True

        DisplayListBox.Items.Clear()

        searchText = SearchTextBox.Text

        For i = 0 To 255
            If InStr(food(i, 0), searchText) <> 0 Then
                blank = False
                DisplayListBox.Items.Add(Me.food(i, 0))
            End If
        Next
        'When item does not exist in the data.
        If blank = True Then
            DisplayLabel.Text = ($"Sorry! No match for {searchText}")
        End If
        'Program exits if zzz is entered into the search text box.
        If searchText = "zzz" Then
            Me.Close()
        End If
    End Sub

    'Filters the data by either Aisle or Category of item.
    Sub FindResult()
        Dim filterSelection As String = FilterComboBox.Text

        DisplayListBox.Items.Clear()

        If filterSelection = " ~Show All~" Then
            DisplayAllItems()
        ElseIf FilterByAisleRadioButton.Checked = True Then
            For i = 0 To 255
                If filterSelection.Trim = food(i, 1) And Me.food(i, 0) <> "" Then
                    DisplayListBox.Items.Add(Me.food(i, 0))
                End If
            Next
        ElseIf FilterByCategoryRadioButton.Checked = True Then
            For i = 0 To 255
                If filterSelection = food(i, 2) And Me.food(i, 0) <> "" Then
                    DisplayListBox.Items.Add(Me.food(i, 0))
                End If
            Next
        End If
        'Displays items in alphabetical order
        DisplayListBox.Sorted = True

    End Sub

    'Calls the location and category of the selected item to be displayed to the customer.
    Sub DisplayLocation()
        Dim item As String
        Dim aisle As String
        Dim category As String

        For i = 0 To 255
            If DisplayListBox.SelectedItem.ToString = food(i, 0) Then
                item = food(i, 0)
                aisle = food(i, 1)
                category = food(i, 2)
            End If
        Next
        DisplayLabel.Text = ($"{item} can be found on aisle {aisle} in the {category} section")
    End Sub

    'Sub to reset form to default 
    Sub Reset()
        FilterByAisleRadioButton.Checked = True
        SearchTextBox.Clear()
        DisplayLabel.Text = ""
        DisplayAllItems()
    End Sub

    'The data search is activated by the search button and on both menu strips.
    Private Sub SearchButton_Click(sender As Object, e As EventArgs) _
        Handles SearchButton.Click, SearchToolStripMenuItem1.Click, SearchToolStripMenuItem2.Click
        SearchByText()
    End Sub

    'The form is exited by the exit button and on both menu strips.
    Private Sub ExitButton_Click(sender As Object, e As EventArgs) _
        Handles ExitButton.Click, ExitToolStripMenuItem1.Click, ExitToolStripMenuItem2.Click
        Me.Close()
    End Sub

    'Button to reset form to default
    Private Sub ClearButton_Click(sender As Object, e As EventArgs) Handles ClearButton.Click
        Reset()
    End Sub

    'Default state of the form. Loads the data file.
    Private Sub StansGroceryForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadDataFile()
        Reset()
    End Sub

    'Activates the differnt filters by selecting the radio buttons for each filter type.
    Private Sub FilterComboBox_SelectedValueChanged(sender As Object, e As EventArgs) Handles FilterComboBox.SelectedValueChanged
        FindResult()
    End Sub

    'Displays the location of the item to the user.
    Private Sub DisplayListBox_SelectedValueChanged(sender As Object, e As EventArgs) Handles DisplayListBox.SelectedValueChanged
        DisplayLocation()
    End Sub

    'Informs the user the purpose of the program.
    Private Sub AboutToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AboutToolStripMenuItem.Click
        MsgBox("Search for items Stan's in Grocery Store")
    End Sub

    'Displays the data of each filter in the combo box. 
    Private Sub FilterByAisleRadioButton_CheckedChanged(sender As Object, e As EventArgs) _
        Handles FilterByAisleRadioButton.CheckedChanged
        ComboBoxDisplay()
    End Sub
End Class
