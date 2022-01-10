using MVVM3.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM3.ViewModel
{
    public class NotesViewModel : BindableBase
    {
        public ObservableCollection<Note> Notes { get; set; }
        public MyICommand AddNoteCommand { get; set; }
        private Note currentNote = new Note();

        public NotesViewModel()
        {
            Notes = new ObservableCollection<Note>();
            AddNoteCommand = new MyICommand(OnAdd);
        }

        public Note CurrentNote
        {
            get { return currentNote; }
            set
            {
                currentNote = value;
                OnPropertyChanged("CurrentNote");
            }
        }

        public void OnAdd()
        {
            CurrentNote.Validate();
            if(CurrentNote.IsValid)
            {
                Notes.Add(new Note()
                {
                    Title = CurrentNote.Title,
                    Description = CurrentNote.Description
                });
            }

        }
    }
}
