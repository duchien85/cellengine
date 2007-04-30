﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Xml;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Runtime.Serialization.Formatters;
using System.IO;

namespace CellGameEdit.PM
{

    [Serializable]
    public partial class ProjectForm : Form , ISerializable
    {
 
        static public String workSpace = "";
        static public String workName = "";

        TreeNode nodeReses;
        TreeNode nodeLevels;
        TreeNode nodeObjects;


        //ArrayList formGroup;
        Hashtable formTable;

        //-----------------------------------------------------------------------------------------------------------------------------------
        // new 
        public ProjectForm()
        {
            InitializeComponent();

           // formGroup = new ArrayList();
            formTable       = new Hashtable();

            nodeReses       = new TreeNode("资源");
            nodeObjects     = new TreeNode("对象");
            nodeLevels      = new TreeNode("场景");
            

            nodeReses.ContextMenuStrip = this.resMenu;
            nodeObjects.ContextMenuStrip = this.objMenu;
            nodeLevels.ContextMenuStrip = this.levelMenu;


            treeView1.Nodes.Add(nodeReses);
            treeView1.Nodes.Add(nodeObjects);
            treeView1.Nodes.Add(nodeLevels);

            treeView1.ExpandAll();

           
            

        }

        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        protected ProjectForm(SerializationInfo info, StreamingContext context)
        {
            InitializeComponent();

            

            nodeReses = (TreeNode)info.GetValue("nodeReses", typeof(TreeNode));
            nodeLevels = (TreeNode)info.GetValue("nodeLevels", typeof(TreeNode));
            try
            {
                nodeObjects = (TreeNode)info.GetValue("nodeObjects", typeof(TreeNode));
            }catch(Exception err){
                nodeObjects = new TreeNode("对象");
            }
            //formGroup = (ArrayList)info.GetValue("formGroup", typeof(ArrayList));

            formTable = (Hashtable)info.GetValue("formTable", typeof(Hashtable));

            nodeReses.ContextMenuStrip = this.resMenu;
            nodeObjects.ContextMenuStrip = this.objMenu;
            nodeLevels.ContextMenuStrip = this.levelMenu;


            foreach(TreeNode node in nodeReses.Nodes)
            {
                node.ContextMenuStrip = this.tileMenu;

                foreach(TreeNode subnode in node.Nodes)
                {
                    subnode.ContextMenuStrip = this.subMenu;
                }
            }
            foreach (TreeNode node in nodeLevels.Nodes)
            {
                node.ContextMenuStrip = this.subMenu;
            }
            foreach (TreeNode node in nodeObjects.Nodes)
            {
                node.ContextMenuStrip = this.subMenu;
            }

            treeView1.Nodes.Add(nodeReses);
            treeView1.Nodes.Add(nodeObjects);
            treeView1.Nodes.Add(nodeLevels);

            treeView1.ExpandAll();


            //// load
            //try
            //{
            //    SoapFormatter formatter = new SoapFormatter();
            //    Stream stream = new FileStream(dir.SelectedPath + "\\Project.cpj", FileMode.Open, FileAccess.Read, FileShare.Read);
            //    prjForm = (ProjectForm)formatter.Deserialize(stream);
            //    stream.Close();
            //    prjForm.MdiParent = this;
            //    prjForm.Show();
            //}
            //catch (Exception err)
            //{
            //    MessageBox.Show("找不到工程文件 Project.cpj " + err.StackTrace);
            //}
        }

        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            RefreshNodeName();

           
                // info.AddValue("formGroup", formGroup);
                info.AddValue("nodeReses", nodeReses);
                info.AddValue("nodeObjects", nodeObjects);
                info.AddValue("nodeLevels", nodeLevels);
                info.AddValue("formTable", formTable);

        }

        //public void Output()
        //{
        //    RefreshNodeName();

        //    System.IO.StringWriter sw = new System.IO.StringWriter();
        //    sw.WriteLine("/* Cell Game Editor by WAZA Zhang */");
        //    sw.WriteLine("/* Email : wazazhang@gmail.com */");

        //    sw.WriteLine("import com.morefuntek.cell.*;");
        //    sw.WriteLine("import com.morefuntek.cell.Game.*;");
        //    sw.WriteLine();

        //    sw.WriteLine("public class Reses {");
        //    Output(nodeReses, sw);
        //    sw.WriteLine("}");

        //    Output(nodeLevels, sw);

        //    Console.WriteLine(sw.ToString());

        //    System.IO.File.WriteAllText(
        //        ProjectForm.workSpace + "\\Reses.java",
        //        sw.ToString());

        //    //

        //}

        //public void Output(TreeNode node, System.IO.StringWriter sw)
        //{
        //    if (formTable[node] != null)
        //    {
        //        //
        //        if (formTable[node].GetType().Equals(typeof(ImagesForm)))
        //        {
        //            ((ImagesForm)formTable[node]).Output(sw);
        //        }
        //        if (formTable[node].GetType().Equals(typeof(MapForm)))
        //        {
        //            ((MapForm)formTable[node]).Output(sw);
        //        }
        //        if (formTable[node].GetType().Equals(typeof(SpriteForm)))
        //        {
        //            ((SpriteForm)formTable[node]).Output(sw);
        //        }

        //        //
        //        if (formTable[node].GetType().Equals(typeof(WorldForm)))
        //        {
        //            ((WorldForm)formTable[node]).Output(sw);
        //        }
        //    }

        //    if (node.Nodes.Count >= 0)
        //    {
        //        foreach (TreeNode sub in node.Nodes)
        //        {
        //            Output(sub, sw);
        //        }
        //    }

        //}

        
        string OutputName ;
        string OutputDir ;
        string OutputDirImage;

        string ImageType;			/* 输出图片格式 默认(*.png) */

        Boolean ImageTile;
        Boolean ImageTileData;

        Boolean ImageGroup;
        Boolean ImageGroupData;

        public void OutputCustom(String fileName)
        {
            try
            {
                initForms();
                RefreshNodeName();

                if (System.IO.File.Exists(fileName))
                {
                    Encoding encoding = Util.GetEncoding(fileName);

                    StreamReader sr = new StreamReader(fileName, encoding);
                    string script = sr.ReadToEnd();
                    sr.Close();


                    string ret = new string(new char[] { '\r', '\n' });

                    // build command
                    OutputName =  Util.getCommandScript(script, "<OUTPUT>");
                    try{
                        if (System.IO.Path.IsPathRooted(OutputName))
                        {
                            OutputDir = System.IO.Path.GetDirectoryName(OutputName);
                        }
                        else
                        {
                            OutputDir = workSpace + "\\" + System.IO.Path.GetDirectoryName(OutputName);
                        }
                    }catch(Exception err){
                        OutputDir = System.IO.Path.GetDirectoryName(workSpace);
                    }
                    if (!System.IO.Directory.Exists(OutputDir))
                    {
                        System.IO.Directory.CreateDirectory(OutputDir);
                    }
                    OutputName = OutputDir + "\\" + System.IO.Path.GetFileName(OutputName);

                    // out image
                    OutputDirImage = Util.getCommandScript(script, "<IMAGE OUTPUT>");
                    if (OutputDirImage == "") OutputDirImage = null;
                    try
                    {
                        if (System.IO.Path.IsPathRooted(OutputDirImage))
                        {
                            OutputDirImage = System.IO.Path.GetDirectoryName(OutputDirImage);
                        }
                        else
                        {
                            OutputDirImage = workSpace + "\\" + System.IO.Path.GetDirectoryName(OutputDirImage);
                        }
                        if (!System.IO.Directory.Exists(OutputDirImage))
                        {
                            System.IO.Directory.CreateDirectory(OutputDirImage);
                        }
                        ImageType = Util.getCommandScript(script, "<IMAGE TYPE>");
                        ImageTile = Util.getCommandScript(script, "<IMAGE TILE>").Equals("true", StringComparison.CurrentCultureIgnoreCase);
                        ImageTileData = Util.getCommandScript(script, "<IMAGE TILE DATA>").Equals("true", StringComparison.CurrentCultureIgnoreCase);
                        ImageGroup = Util.getCommandScript(script, "<IMAGE GROUP>").Equals("true", StringComparison.CurrentCultureIgnoreCase);
                        ImageGroupData = Util.getCommandScript(script, "<IMAGE GROUP DATA>").Equals("true", StringComparison.CurrentCultureIgnoreCase);
                    }
                    catch (Exception err)
                    {
                        OutputDirImage = System.IO.Path.GetDirectoryName(workSpace);
                        ImageType = "";
                        ImageTile = false;
                        ImageTileData = false;
                        ImageGroup = false;
                        ImageGroupData = false;
                    }
                   
                    script = fillScriptNode(script);


                    script = script.Insert(0, "/* Email : wazazhang@gmail.com */" + ret);
                    script = script.Insert(0, "/* Cell Game Editor by WAZA Zhang */" + ret);
                    script = script.Insert(0, "/* Encoding : " + encoding.EncodingName + " */" + ret);

                    Console.WriteLine("");
                    Console.WriteLine(script);

                    System.IO.File.WriteAllText(
                        OutputName,
                        script,
                        encoding
                        );

                    Console.WriteLine(ret + "Output --> : " + script.Length + " (Chars)");
                    Console.WriteLine("");
                }
                else
                {
                    Console.WriteLine("Error : " + fileName + " : 不存在!");
                }
            }
            catch (Exception err) { MessageBox.Show(err.StackTrace); }

        }

        ArrayList FormsImages = new ArrayList();
        ArrayList FormsMap = new ArrayList();
        ArrayList FormsSprite = new ArrayList();
        ArrayList FormsWorld = new ArrayList();

        public string fillScriptSub(string src, string start, string end, ArrayList forms)
        {
            string script = src.Substring(0, src.Length);
            string sub = Util.getTrunk(script, start, end);
            if (sub == null) return null;

            ArrayList scripts = new ArrayList();
            try
            {
                for (int i = 0; i < forms.Count; i++)
                {

                    StringWriter output = new StringWriter();
                    //
                    if (forms[i].GetType().Equals(typeof(ImagesForm)))
                    {
                        ((ImagesForm)forms[i]).OutputCustom(i, sub, output, OutputDirImage, ImageType,ImageTile,ImageTileData,ImageGroup,ImageGroupData);
                        Console.WriteLine("Output : " + ((ImagesForm)forms[i]).id + " -> " + output.ToString().Length + "(Chars)");
                    }
                    if (forms[i].GetType().Equals(typeof(MapForm)))
                    {
                        ((MapForm)forms[i]).OutputCustom(i, sub, output);
                        Console.WriteLine("Output : " + ((MapForm)forms[i]).id + " -> " + output.ToString().Length + "(Chars)");
                    }
                    if (forms[i].GetType().Equals(typeof(SpriteForm)))
                    {
                        ((SpriteForm)forms[i]).OutputCustom(i, sub, output);
                        Console.WriteLine("Output : " + ((SpriteForm)forms[i]).id + " -> " + output.ToString().Length + "(Chars)");
                    }
                    if (forms[i].GetType().Equals(typeof(WorldForm)))
                    {
                        ((WorldForm)forms[i]).OutputCustom(i, sub, output);
                        Console.WriteLine("Output : " + ((WorldForm)forms[i]).id + " -> " + output.ToString().Length + "(Chars)");
                    }
                    //
                    scripts.Add(output.ToString());

                }
            }
            catch (Exception err) { MessageBox.Show(err.StackTrace); }

            script = Util.replaceSubTrunksScript(script, start, end, (string[])scripts.ToArray(typeof(string)));
                
            return script;
        }

        public string fillScriptNode(string src)
        {
            string script = src.Substring(0, src.Length);

            try
            {
                // build resource trunk
                string resource = Util.getTrunk(script, "#<RESOURCE>", "#<END RESOURCE>");
                if (resource != null)
                {
                    bool fix = false;
                    do
                    {
                        fix = false;
                        string images = fillScriptSub(resource, "#<IMAGES>", "#<END IMAGES>", FormsImages);
                        if (images != null) { resource = images; fix = true; }

                        string map = fillScriptSub(resource, "#<MAP>", "#<END MAP>", FormsMap);
                        if (map != null) { resource = map; fix = true; }

                        string sprite = fillScriptSub(resource, "#<SPRITE>", "#<END SPRITE>", FormsSprite);
                        if (sprite != null) { resource = sprite; fix = true; }

                    } while (fix);
                }
                resource = Util.replaceKeywordsScript(resource, "#<RESOURCE>", "#<END RESOURCE>",
                    new string[] { "<RES IMAGES COUNT>", "<RES MAP COUNT>", "<RES SPRITE COUNT>" },
                    new string[] { FormsImages.Count.ToString(), FormsMap.Count.ToString(), FormsSprite.Count.ToString()});
                script = Util.replaceSubTrunksScript(script, "#<RESOURCE>", "#<END RESOURCE>", new string[] { resource });
            }
            catch (Exception err) { MessageBox.Show(err.StackTrace); }

            //build world trunk
            try
            {
                string level = Util.getTrunk(script, "#<LEVEL>", "#<END LEVEL>");
                if (level != null)
                {
                    bool fix = false;
                    do
                    {
                        fix = false;
                        string world = fillScriptSub(level, "#<WORLD>", "#<END WORLD>", FormsWorld);
                        if (world != null) { level = world; fix = true; }

                    } while (fix);
                }
                level = Util.replaceKeywordsScript(level, "#<LEVEL>", "#<END LEVEL>",
                    new string[] { "<LEVEL WORLD COUNT>" },
                    new string[] { FormsWorld.Count.ToString() });
                script = Util.replaceSubTrunksScript(script, "#<LEVEL>", "#<END LEVEL>", new string[] { level });
            }
            catch (Exception err) { MessageBox.Show(err.StackTrace); }

            return script;
        }


   

        public void initForms()
        {
            FormsImages.Clear();
            FormsMap.Clear();
            FormsSprite.Clear();
            FormsWorld.Clear();

            initForms(nodeReses);
            initForms(nodeLevels);

        }
        public void initForms(TreeNode node)
        {
            if (formTable[node] != null)
            {
                //
                if (formTable[node].GetType().Equals(typeof(ImagesForm)))
                {
                    FormsImages.Add(((ImagesForm)formTable[node]));
                }
                if (formTable[node].GetType().Equals(typeof(MapForm)))
                {
                    FormsMap.Add(((MapForm)formTable[node]));
                }
                if (formTable[node].GetType().Equals(typeof(SpriteForm)))
                {
                    FormsSprite.Add(((SpriteForm)formTable[node]));
                }

                //
                if (formTable[node].GetType().Equals(typeof(WorldForm)))
                {
                    FormsWorld.Add(((WorldForm)formTable[node]));
                }
            }

            if (node.Nodes.Count >= 0)
            {
                foreach (TreeNode sub in node.Nodes)
                {
                    initForms(sub);
                }
            }

        }


        public void RefreshNodeName()
        {
            RefreshNodeName(nodeReses);
            RefreshNodeName(nodeLevels);
        }

        public void RefreshNodeName(TreeNode node)
        {
            if (formTable[node] != null)
            {
                //
                if (formTable[node].GetType().Equals(typeof(ImagesForm)))
                {
                    ((ImagesForm)formTable[node]).Text = node.Text;
                    ((ImagesForm)formTable[node]).id = node.Text;
                }
                if (formTable[node].GetType().Equals(typeof(MapForm)))
                {
                    ((MapForm)formTable[node]).Text = node.Text;
                    ((MapForm)formTable[node]).id = node.Text;
                }
                if (formTable[node].GetType().Equals(typeof(SpriteForm)))
                {
                    ((SpriteForm)formTable[node]).Text = node.Text;
                    ((SpriteForm)formTable[node]).id = node.Text;
                }

                //
                if (formTable[node].GetType().Equals(typeof(WorldForm)))
                {
                    ((WorldForm)formTable[node]).Text = node.Text;
                    ((WorldForm)formTable[node]).id = node.Text;
                }
            }

            if (node.Nodes.Count >= 0)
            {
                foreach (TreeNode sub in node.Nodes)
                {
                    RefreshNodeName(sub);
                }
            }
         
        }

        private Form getForm(TreeNode key)
        {
            try
            {
                if (formTable[key] != null)
                {
                    if (formTable[key].GetType().Equals(typeof(ImagesForm)) ||
                        formTable[key].GetType().Equals(typeof(SpriteForm)) ||
                        formTable[key].GetType().Equals(typeof(MapForm))    ||
                        formTable[key].GetType().Equals(typeof(WorldForm))  ||
                        formTable[key].GetType().Equals(typeof(ObjectForm)))
                    {
                        return (Form)formTable[key];
                    }
                }
            }
            catch (Exception err)
            {
            }
            return null;
        }

        private void ProjectForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            foreach (TreeNode key in formTable.Keys)
            {
                
                if (getForm(key) != null)
                {
                    getForm(key).Close();
                    getForm(key).Dispose();
                }
            }

        }

        private void ProjectForm_Shown(object sender, EventArgs e)
        {
            RefreshNodeName();
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            this.treeView1.SelectedNode = e.Node;
        }


        private void treeView1_BeforeLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if(e.Node.Parent==null){
                nodeReses.EndEdit(true);
            }
        }

        private void treeView1_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
           

            if (getForm(treeView1.SelectedNode) != null)
            {
                if (getForm(treeView1.SelectedNode).GetType().Equals(typeof(ImagesForm)))
                { 
                    ((ImagesForm)getForm(treeView1.SelectedNode)).id = e.Label; 
                }
                if (getForm(treeView1.SelectedNode).GetType().Equals(typeof(SpriteForm)))
                { 
                    ((SpriteForm)getForm(treeView1.SelectedNode)).id = e.Label; 
                }
                if (getForm(treeView1.SelectedNode).GetType().Equals(typeof(MapForm)))
                { 
                    ((MapForm)getForm(treeView1.SelectedNode)).id = e.Label; 
                }
                if (getForm(treeView1.SelectedNode).GetType().Equals(typeof(WorldForm)))
                { 
                    ((WorldForm)getForm(treeView1.SelectedNode)).id = e.Label; 
                }

            }

            RefreshNodeName();
        }

        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (getForm(treeView1.SelectedNode) != null)
            {
                getForm(treeView1.SelectedNode).MdiParent = this.MdiParent;
                getForm(treeView1.SelectedNode).Show();
                getForm(treeView1.SelectedNode).Select();
            }
           
            
        }

        private void treeView1_ItemDrag(object sender, ItemDragEventArgs e)
        {
            if (getForm((TreeNode)e.Item) != null)
            {

                if (getForm((TreeNode)e.Item).GetType().Equals(typeof(SpriteForm)))
                {
                    DoDragDrop((SpriteForm)getForm((TreeNode)e.Item), DragDropEffects.Link);
                    //Console.WriteLine("Spr drag");
                }
                if (getForm((TreeNode)e.Item).GetType().Equals(typeof(MapForm)))
                {
                    DoDragDrop((MapForm)getForm((TreeNode)e.Item), DragDropEffects.Link);
                    //Console.WriteLine("map drag");
                }

            }
           
        }


        //------------------------------------------------------------------------------------------------------------------------------------

#region resMenu
        private void 添加图片组ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            String name = "unamed_Tile";
            TextDialog nameDialog = new TextDialog(name);
            while (nameDialog.ShowDialog() == DialogResult.OK)
            {
                name = nameDialog.getText();
                if (treeView1.SelectedNode.Nodes.ContainsKey(name))
                {
                    MessageBox.Show("已经有　" + name + " 这个名字了");
                    continue;
                }
                
                ImagesForm form = new ImagesForm(name);
                TreeNode node = new TreeNode(name);
                node.Name = name;
                formTable.Add(node, form);

                node.ContextMenuStrip = this.tileMenu;
                this.treeView1.SelectedNode.Nodes.Add(node);
                this.treeView1.SelectedNode.ExpandAll();
                form.MdiParent = this.MdiParent;
                form.Show();

                try
                {
                    String dir = workSpace + "\\" + form.id;
                    if (System.IO.Directory.Exists(dir))
                    {
                        System.IO.Directory.Delete(dir,true);
                    }
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.TargetSite+":"+err.StackTrace);
                }
                break;
            }

        }
#endregion

#region levelMenu

        private void 添加场景ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            String name = "unamed_Level";
            TextDialog nameDialog = new TextDialog(name);
            while (nameDialog.ShowDialog() == DialogResult.OK)
            {
                name = nameDialog.getText();
                if (treeView1.SelectedNode.Nodes.ContainsKey(name))
                {
                    MessageBox.Show("已经有　" + name + " 这个名字了");
                    continue;
                }

                WorldForm form = new WorldForm(name);
                TreeNode node = new TreeNode(name);
                node.Name = name;
                formTable.Add(node, form);

                node.ContextMenuStrip = this.subMenu;
                this.treeView1.SelectedNode.Nodes.Add(node);
                this.treeView1.SelectedNode.ExpandAll();
                form.MdiParent = this.MdiParent;
                form.Show();
                break;
            }

        }
#endregion

#region objMenu
        private void 添加对象ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            String name = "unamed_Object";
            TextDialog nameDialog = new TextDialog(name);
            while (nameDialog.ShowDialog() == DialogResult.OK)
            {
                name = nameDialog.getText();
                if (treeView1.SelectedNode.Nodes.ContainsKey(name))
                {
                    MessageBox.Show("已经有　" + name + " 这个名字了");
                    continue;
                }

                ObjectForm form = new ObjectForm(name);
                TreeNode node = new TreeNode(name);
                node.Name = name;
                formTable.Add(node, form);

                node.ContextMenuStrip = this.subMenu;
                this.treeView1.SelectedNode.Nodes.Add(node);
                this.treeView1.SelectedNode.ExpandAll();
                form.MdiParent = this.MdiParent;
                form.Show();
                break;
            }
        }

#endregion

        //------------------------------------------------------------------------------------------------------------------------------------
        
#region tileMenu

        private void 打开ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (getForm(treeView1.SelectedNode) != null)
            {
                getForm(treeView1.SelectedNode).MdiParent = this.MdiParent;
                getForm(treeView1.SelectedNode).Show();
                getForm(treeView1.SelectedNode).Select();
            }
           
        }

        private void 精灵ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            String name = "unamed_Sprite";
            TextDialog nameDialog = new TextDialog(name);
            while (nameDialog.ShowDialog() == DialogResult.OK)
            {
                name = nameDialog.getText();
                if (treeView1.SelectedNode.Nodes.ContainsKey(name))
                {
                    MessageBox.Show("已经有　" + name + " 这个名字了");
                    continue;
                }
                SpriteForm form = ((ImagesForm)getForm(treeView1.SelectedNode)).createSpriteForm(name);
                if (form != null)
                {
                    TreeNode node = new TreeNode(name);
                    node.Name = name;
                    formTable.Add(node, form);
                    node.ContextMenuStrip = this.subMenu;
                    this.treeView1.SelectedNode.Nodes.Add(node);
                    this.treeView1.SelectedNode.ExpandAll();
                    form.MdiParent = this.MdiParent;
                    form.Show();
                }
                break;
            }
        }

        private void 地图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            String name = "unamed_Map";
            TextDialog nameDialog = new TextDialog(name);
            while (nameDialog.ShowDialog() == DialogResult.OK)
            {
                name = nameDialog.getText();
                if (treeView1.SelectedNode.Nodes.ContainsKey(name))
                {
                    MessageBox.Show("已经有　" + name + " 这个名字了");
                    continue;
                }
                MapForm form = ((ImagesForm)getForm(treeView1.SelectedNode)).createMapForm(name);
                if (form != null)
                {
                    TreeNode node = new TreeNode(name);
                    node.Name = name;
                    formTable.Add(node, form);
                    node.ContextMenuStrip = this.subMenu;
                    this.treeView1.SelectedNode.Nodes.Add(node);
                    this.treeView1.SelectedNode.ExpandAll();
                    form.MdiParent = this.MdiParent;
                    form.Show();
                    
                }
                break;
            }

        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (getForm(treeView1.SelectedNode)!=null)
            {
                while (treeView1.SelectedNode.Nodes.Count > 0)
                {
                    getForm(treeView1.SelectedNode.Nodes[0]).Enabled = false;
                    getForm(treeView1.SelectedNode.Nodes[0]).Dispose();
                    //getForm(treeView1.SelectedNode.Nodes[0]).Hide();
                    formTable.Remove(treeView1.SelectedNode.Nodes[0]);
                    treeView1.SelectedNode.Nodes.RemoveAt(0);
                }

                getForm(treeView1.SelectedNode).Enabled = false;
                getForm(treeView1.SelectedNode).Dispose();
                //getForm(treeView1.SelectedNode).Hide();
                formTable.Remove(treeView1.SelectedNode);
                treeView1.SelectedNode.Parent.Nodes.Remove(treeView1.SelectedNode);
            
           
            }
           
        }

#endregion

        //------------------------------------------------------------------------------------------------------------------------------------

#region subMenu

        private void 打开ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (getForm(treeView1.SelectedNode) != null)
            {
                getForm(treeView1.SelectedNode).MdiParent = this.MdiParent;
                getForm(treeView1.SelectedNode).Show();
                getForm(treeView1.SelectedNode).Select();
            }
            
        }

        private void 删除ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (getForm(treeView1.SelectedNode) != null)
            {

                getForm(treeView1.SelectedNode).Enabled = false;
                getForm(treeView1.SelectedNode).Dispose();
               
                //getForm(treeView1.SelectedNode).Hide();
                formTable.Remove(treeView1.SelectedNode);
                treeView1.SelectedNode.Parent.Nodes.Remove(treeView1.SelectedNode);

               
            }
        }

#endregion



    }
}