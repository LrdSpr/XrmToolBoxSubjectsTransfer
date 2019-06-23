using System;
using System.Collections.Generic;
using System.Windows.Forms;
using XrmToolBox.Extensibility;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk;
using System.Runtime.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Reflection;

namespace TransferCaseSubjects
{
    public partial class TransferCaseSubjects : PluginControlBase
    {
        [Serializable]
        public class SubjectItem
        {
            public Guid Id;
            public SubjectItem ParentItem;
            public string Title;
            public int Featuremask;
            public TreeNode Node;
            public List<SubjectItem> ChildItems = new List<SubjectItem>();
        }


        List<SubjectItem> SubjectsToDelete = new List<SubjectItem>();
        List<SubjectItem> SubjectsToEdit = new List<SubjectItem>();
        Dictionary<Guid, SubjectItem> SubjectCache = new Dictionary<Guid, SubjectItem>();
        List<SubjectItem> Subjects = new List<SubjectItem>();
        Dictionary<Guid, Guid> CrmCache = new Dictionary<Guid, Guid>();

        public TransferCaseSubjects()
        {
            InitializeComponent();
        }

        private void ConstructCasheFromSubjects(List<SubjectItem> subjects)
        {
            foreach (var item in subjects)
            {
                SubjectCache.Add(item.Id, item);
                if (item.ChildItems.Count > 0)
                {
                    ConstructCasheFromSubjects(item.ChildItems);
                }
            }
        }


        private List<Guid> BuildParentsList(IOrganizationService proxy)
        {
            List<Guid> currentCache = new List<Guid>();
            QueryExpression qE = new QueryExpression("subject");
            qE.ColumnSet = new ColumnSet(true);
            var subjects = proxy.RetrieveMultiple(qE);

            foreach (Entity enity in subjects.Entities)
            {
                if (enity.Contains("parentsubject"))
                {
                    currentCache.Add(((EntityReference)enity["parentsubject"]).Id);
                }
            }
            return currentCache;
        }


        private void LoadSubjects(IOrganizationService proxy)
        {
            SubjectCache.Clear();
            Subjects.Clear();
            SubjectsToDelete.Clear();
            QueryExpression qE = new QueryExpression("subject");
            qE.ColumnSet = new ColumnSet(true);
            var subjects = proxy.RetrieveMultiple(qE);
            ConstructSubjectsHierarchy(subjects.Entities);
        }


        public bool FindParentSubjectItem(DataCollection<Entity> subjects, Guid subjectId)
        {
            foreach (var subject in subjects)
            {
                if (subject.Id == subjectId)
                {
                    AddSubjectToHierarchy(subjects, subject);
                    return true;
                }
            }
            return false;
        }

        public void AddSubjectToHierarchy(DataCollection<Entity> subjects, Entity subject)
        {
            if (subject.Contains("parentsubject"))
            {
                if (!SubjectCache.ContainsKey(((EntityReference)subject["parentsubject"]).Id))
                {
                    if (FindParentSubjectItem(subjects, ((EntityReference)subject["parentsubject"]).Id))
                    {
                        var item = new SubjectItem();
                        item.ParentItem = SubjectCache[((EntityReference)subject["parentsubject"]).Id];
                        item.Id = subject.Id;
                        item.Title = subject["title"].ToString();
                        item.Featuremask = Convert.ToInt32(subject["featuremask"]);
                        item.Node = item.ParentItem.Node.Nodes.Add(item.Title);
                        item.Node.Tag = item.Id;
                        SubjectCache[((EntityReference)subject["parentsubject"]).Id].ChildItems.Add(item);
                        SubjectCache.Add(subject.Id, item);
                    }
                    else
                    {
                        throw new Exception("Can't find parent for SubjectId" + subject.Id);
                    }
                }
                else
                {
                    if (!SubjectCache.ContainsKey(subject.Id))
                    {
                        var item = new SubjectItem();
                        item.ParentItem = SubjectCache[((EntityReference)subject["parentsubject"]).Id];
                        item.Id = subject.Id;
                        item.Title = subject["title"].ToString();
                        item.Featuremask = Convert.ToInt32(subject["featuremask"]);
                        item.Node = item.ParentItem.Node.Nodes.Add(item.Title);
                        item.Node.Tag = item.Id;
                        SubjectCache[((EntityReference)subject["parentsubject"]).Id].ChildItems.Add(item);
                        SubjectCache.Add(subject.Id, item);

                    }
                }
            }
            else
            {
                if (!SubjectCache.ContainsKey(subject.Id))
                {
                    var item = new SubjectItem();
                    item.ParentItem = null;
                    item.Id = subject.Id;
                    item.Title = subject["title"].ToString();
                    item.Featuremask = Convert.ToInt32(subject["featuremask"]);
                    item.Node = new TreeNode(item.Title);
                    item.Node.Tag = item.Id;
                    Subjects.Add(item);
                    SubjectCache.Add(subject.Id, item);
                }
            }
        }

        public void ConstructSubjectsHierarchy(DataCollection<Entity> subjects)
        {
            foreach (var subject in subjects)
            {
                AddSubjectToHierarchy(subjects, subject);
            }
        }

        public void CreateChildSubjectsItems(IOrganizationService service, Guid ParentId, List<SubjectItem> childItems)
        {
            foreach (var subject in childItems)
            {
                Entity subjectRecord = new Entity("subject");
                subjectRecord.Id = subject.Id;
                subjectRecord["title"] = subject.Title;
                subjectRecord["featuremask"] = subject.Featuremask;
                subjectRecord["parentsubject"] = new EntityReference("subject", ParentId);
                if (!CrmCache.ContainsKey(subjectRecord.Id))
                    service.Create(subjectRecord);
                CreateChildSubjectsItems(service, subject.Id, subject.ChildItems);
            }
        }

        private Dictionary<Guid, Guid> GetExisitingItemsFromCrm(IOrganizationService proxy)
        {
            Dictionary<Guid, Guid> currentCache = new Dictionary<Guid, Guid>();
            QueryExpression qE = new QueryExpression("subject");
            qE.ColumnSet = new ColumnSet(true);
            var subjects = proxy.RetrieveMultiple(qE);

            foreach (Entity enity in subjects.Entities)
            {
                if (enity.Contains("parentsubject"))
                {
                    currentCache.Add(enity.Id, ((EntityReference)enity["parentsubject"]).Id);
                }
                else
                {
                    currentCache.Add(enity.Id, Guid.Empty);
                }
            }
            return currentCache;
        }

        public string ShowInsertDialog(string text, string caption)
        {
            Form prompt = new Form()
            {
                Width = 500,
                Height = 150,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterScreen
            };
            System.Windows.Forms.Label textLabel = new System.Windows.Forms.Label() { Left = 50, Top = 20, Text = text };
            TextBox textBox = new TextBox() { Left = 50, Top = 50, Width = 400 };
            Button confirmation = new Button() { Text = "Ok", Left = 350, Width = 100, Top = 70, DialogResult = DialogResult.OK };
            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            prompt.AcceptButton = confirmation;

            return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : "";
        }

        private void MarkSubjectsForDeletion(SubjectItem item)
        {
            if (item.ChildItems.Count > 0)
            {
                foreach (var child in item.ChildItems)
                {
                    if (child.ChildItems.Count > 0)
                        MarkSubjectsForDeletion(child);
                    SubjectsToDelete.Add(child);
                }
            }
            else
            {
                SubjectsToDelete.Add(item);
            }
        }

        private void loadHierarchy(object sender, EventArgs e)
        {
            ExecuteMethod(LoadSubjectsFromCRM);
        }

        private void updateInCrm(object sender, EventArgs e)
        {
            ExecuteMethod(UpdateSubjectsInCRM);
        }

        private void loadHierarchyFromFile(object sender, EventArgs e)
        {
            SubjectCache.Clear();
            Subjects.Clear();

            openHierarchyDialog.Filter = "Data Files (*.dat) |*.dat|All files(*.*) |*.*";
            if (openHierarchyDialog.ShowDialog() == DialogResult.OK && openHierarchyDialog.FileName != null)
            {
                IFormatter formatter = new BinaryFormatter();

                Subjects = BinaryFormatterHelper.Read<List<SubjectItem>>(openHierarchyDialog.FileName, 
                    Assembly.GetExecutingAssembly());

                ConstructCasheFromSubjects(Subjects);

                subjectsTreeView.BeginUpdate();
                foreach (var item in Subjects)
                {
                    if (item.ParentItem == null)
                        subjectsTreeView.Nodes.Add(item.Node);
                }
                subjectsTreeView.EndUpdate();
            }
        }

        private void saveHierarchy(object sender, EventArgs e)
        {
            saveHierarchyDialog.Filter = "Data Files (*.dat) |*.dat|All files(*.*) |*.*";
            if (saveHierarchyDialog.ShowDialog() == DialogResult.OK && saveHierarchyDialog.FileName != null)
            {
                BinaryFormatterHelper.Write(Subjects, saveHierarchyDialog.FileName);
            }
        }

        public override void ClosingPlugin(PluginCloseInfo info)
        {
            base.ClosingPlugin(info);
        }

        public void LoadSubjectsFromCRM()
        {
            WorkAsync(new WorkAsyncInfo
            {
                Message = "Retrieving subjects from CRM...",
                Work = (w, e) =>
                {
                    LoadSubjects(Service);
                },
                PostWorkCallBack = e =>
                {

                    if (e.Error != null)
                    {
                        MessageBox.Show(e.Error.Message, "Error",
                                System.Windows.Forms.MessageBoxButtons.OK,
                                System.Windows.Forms.MessageBoxIcon.Error);

                    }
                    else
                    {
                        subjectsTreeView.BeginUpdate();
                        subjectsTreeView.Nodes.Clear();
                        foreach (var item in Subjects)
                        {
                            if (item.ParentItem == null)
                                subjectsTreeView.Nodes.Add(item.Node);
                        }
                        subjectsTreeView.EndUpdate();
                    }
                },
                AsyncArgument = null,
                IsCancelable = false,
                MessageWidth = 340,
                MessageHeight = 150
            });
        }

        public void UpdateSubjectsInCRM()
        {
            WorkAsync(new WorkAsyncInfo
            {
                Message = "Updating data in CRM...",
                Work = (w, e) =>
                {
                    CrmCache.Clear();
                    CrmCache = GetExisitingItemsFromCrm(Service);

                    foreach (var subject in Subjects)
                    {
                        Entity subjectRecord = new Entity("subject");
                        subjectRecord.Id = subject.Id;
                        subjectRecord["title"] = subject.Title;
                        subjectRecord["featuremask"] = subject.Featuremask;
                        if (!CrmCache.ContainsKey(subjectRecord.Id))
                            Service.Create(subjectRecord);
                        CreateChildSubjectsItems(Service, subject.Id, subject.ChildItems);
                    }

                    //Update cache
                    CrmCache.Clear();
                    CrmCache = GetExisitingItemsFromCrm(Service);

                    foreach (var item in SubjectsToEdit)
                    {
                        if (CrmCache.ContainsKey(item.Id))
                        {
                            Entity subjectRecord = new Entity("subject");
                            subjectRecord.Id = item.Id;
                            subjectRecord["title"] = item.Title;
                            Service.Update(subjectRecord);
                        }
                        else
                        {
                                throw new KeyNotFoundException("Edit operation. No such subject '" + item.Id + "' record found in the target system");
                        }
                    }
                    SubjectsToEdit.Clear();
                    foreach (var item in SubjectsToDelete)
                    {
                        if (CrmCache.ContainsKey(item.Id))
                        {
                            Service.Delete("subject", item.Id);
                        }
                        else
                        {
                                throw new KeyNotFoundException("Delete operation. No such subject '" + item.Id +"' record found in the target system");
                        }
                    }
                    SubjectsToDelete.Clear();
                },
                PostWorkCallBack = e =>
                {
                    if (e.Error != null)
                    {
                        MessageBox.Show(e.Error.Message, "Error",
                                System.Windows.Forms.MessageBoxButtons.OK,
                                System.Windows.Forms.MessageBoxIcon.Error);

                    }else
                    {

                        ExecuteMethod(LoadSubjectsFromCRM);

                        MessageBox.Show("Data was successfully updated.", "Operation",
                               System.Windows.Forms.MessageBoxButtons.OK,
                               System.Windows.Forms.MessageBoxIcon.Information);
                    }
                },
                AsyncArgument = null,
                IsCancelable = false,
                MessageWidth = 340,
                MessageHeight = 150
            });
        }

        private void addSubjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (subjectsTreeView.SelectedNode != null)
            {

                if (subjectsTreeView.SelectedNode.Tag is Guid)
                {
                    Guid Sid = (Guid)subjectsTreeView.SelectedNode.Tag;
                    if (Sid != null && Sid != Guid.Empty)
                    {
                        var subjectItem = SubjectCache[Sid];

                        string name = ShowInsertDialog("Please enter title:         ", "Add child subject");

                        if (!string.IsNullOrEmpty(name))
                        {
                            Guid Id = Guid.NewGuid();
                            var item = new SubjectItem
                            {
                                ParentItem = SubjectCache[Sid],
                                Id = Id,
                                Title = name,
                                Featuremask = 1,
                                Node = subjectsTreeView.SelectedNode.Nodes.Add(name)
                            };
                            item.Node.Tag = Id;
                            SubjectCache[Sid].ChildItems.Add(item);
                            SubjectCache.Add(Id, item);
                        }
                    }
                }
            }
            else
            {
                string name = ShowInsertDialog("Please enter title:         ", "Add Root Subject");
                if (!string.IsNullOrEmpty(name))
                {
                    var item = new SubjectItem
                    {
                        ParentItem = null,
                        Id = Guid.NewGuid(),
                        Title = name,
                        Featuremask = 1
                    };
                    item.Node = new TreeNode(item.Title);
                    item.Node.Tag = item.Id;
                    Subjects.Add(item);
                    SubjectCache.Add(item.Id, item);
                    subjectsTreeView.Nodes.Add(item.Node);
                }
            }
        }

        private void removeSubjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (subjectsTreeView.SelectedNode != null)
            {
                if (System.Windows.Forms.MessageBox.Show("Do you want to delete subject and all childs?", "Delete Subject",
                                System.Windows.Forms.MessageBoxButtons.OKCancel,
                                System.Windows.Forms.MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    if (subjectsTreeView.SelectedNode.Tag is Guid)
                    {
                        Guid Sid = (Guid)subjectsTreeView.SelectedNode.Tag;
                        if (Sid != null && Sid != Guid.Empty)
                        {
                            var subjectItem = SubjectCache[Sid];
                            MarkSubjectsForDeletion(subjectItem);
                            if (!SubjectsToDelete.Contains(subjectItem))
                            {
                                SubjectsToDelete.Add(subjectItem);
                            }
                            subjectsTreeView.BeginUpdate();
                            subjectsTreeView.Nodes.Remove(subjectsTreeView.SelectedNode);
                            subjectsTreeView.EndUpdate();
                        }
                    }
                }
            }
        }

        private void TransferCaseSubjects_Load(object sender, EventArgs e)
        {
            ShowInfoNotification("Please select an item to update and use the context menu of the tree view to add or remove items. Please click 'Update Subjects in CRM.' to reflect your changes..", new Uri("https://www.xrmtoolbox.com"), 32);
        }

        private void editSubjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (subjectsTreeView.SelectedNode != null)
            {
                if (subjectsTreeView.SelectedNode.Tag is Guid)
                {
                    Guid Sid = (Guid)subjectsTreeView.SelectedNode.Tag;
                    if (Sid != null && Sid != Guid.Empty)
                    {

                        string name = ShowInsertDialog("New title:         ", "Edit Subject");

                        var subjectItem = SubjectCache[Sid];
                        subjectItem.Title = name;

                        if (!SubjectsToEdit.Contains(subjectItem))
                        {
                            SubjectsToEdit.Add(subjectItem);
                        }
                        else
                        {
                            SubjectsToEdit.Remove(subjectItem);
                            SubjectsToEdit.Add(subjectItem);
                        }

                        subjectsTreeView.BeginUpdate();
                        subjectsTreeView.SelectedNode.Text = name;
                        subjectsTreeView.EndUpdate();

                    }
                }
            }
        }
    }
}