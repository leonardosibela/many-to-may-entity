using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Data.Entity.Migrations;

namespace Cursos
{
    public partial class MainWindow : Window
    {
        public List<Course> Courses { get; set; }
        private SchoolDBContext dbContext;
        public MainWindow()
        {
            InitializeComponent();
            dbContext = new SchoolDBContext();
            Courses = dbContext.Courses.ToList();
            DataContext = this;
            fillCoursesComboBox();
            lstCourses.ItemsSource = Courses;
            updateDatagrid();
        }

        private void fillCoursesComboBox()
        {
            cboCourses.ItemsSource = Courses;
        }

        private void btnRegistry_Click(object sender, RoutedEventArgs e)
        {
            if (!isFormValid())
            {
                displayInvalidFormMessage();
                return;
            }

            Student student = getStudentFromForm();
            dbContext.Students.Add(student);
            dbContext.SaveChanges();

            updateDatagrid();
            clearFrom();
        }

        private void clearFrom()
        {
            txtName.Clear();
            lstCourses.ItemsSource = Courses;
            lstCourses.UnselectAll();
        }

        private void updateDatagrid()
        {
            dtgStudents.ItemsSource = dbContext.Students.ToList();
        }

        private Student getStudentFromForm()
        {
            Student student = new Student();
            student.StudentName = txtName.Text;

            foreach (Course course in lstCourses.SelectedItems)
            {
                student.Courses.Add(course);
            }

            return student;
        }

        private void displayInvalidFormMessage()
        {
            if (String.IsNullOrEmpty(txtName.Text))
            {
                MessageBox.Show("Favor preencher o nome do aluno", "Erro Preenchimento", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if ((lstCourses.SelectedItems.Count == 0))
            {
                MessageBox.Show("Favor selecionar pelo menos um curso", "Erro Preenchimento", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private bool isFormValid()
        {
            return
               !String.IsNullOrEmpty(txtName.Text) &&
               !(lstCourses.SelectedItems.Count == 0);
        }

        private void btnClean_Click(object sender, RoutedEventArgs e)
        {
            txtName.Clear();
            lstCourses.UnselectAll();
            dtgStudents.SelectedIndex = -1;
        }

        private void dtgStudents_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Student selectedStudent = (Student)dtgStudents.SelectedItem;
            if (selectedStudent != null)
            {
                displayStudentData(selectedStudent);
            }
            else
            {
                clearFrom();
            }
        }

        private void displayStudentData(Student student)
        {
            txtName.Text = student.StudentName;
            lstCourses.SelectedItems.Clear();
            student.Courses.ToList().ForEach(s => lstCourses.SelectedItems.Add(s));
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            Student selectedStudent = (Student)dtgStudents.SelectedItem;

            if (selectedStudent == null)
            {
                MessageBox.Show("É necessário selecionar um aluno", "Erro ao remover", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            dbContext.Students.Attach(selectedStudent);
            dbContext.Students.Remove(selectedStudent);
            dbContext.SaveChanges();

            updateDatagrid();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (dtgStudents.SelectedIndex == -1)
            {
                MessageBox.Show("Aluno não selecionado", "Favor selecionar um aluno", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (String.IsNullOrEmpty(txtName.Text))
            {
                MessageBox.Show("Nome não selecionado", "Favor selecionar um nome para o aluno", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Student student = (Student)dtgStudents.SelectedItem;
            student.StudentName = txtName.Text;

            student.Courses.Clear();

            foreach (Course course in lstCourses.SelectedItems)
            {
                student.Courses.Add(course);
            }

            dbContext.Students.AddOrUpdate(student);
            dbContext.SaveChanges();
        }
    }
}
