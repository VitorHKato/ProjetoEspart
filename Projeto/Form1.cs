using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Projeto
{
    public partial class Form1 : Form
    {
        TAREFA model = new TAREFA();

        public Form1()
        {
            InitializeComponent();
            Limpar();
            this.Text = "TODO App";
            preencherDataGridView();
        }

        void Limpar()
        {
            txtDescricao.Text = txtSituacao.Text = "";
            btnExcluir.Enabled = false;
            btnAddTarefa.Text = "Adicionar";
            label3.Text = "Situação";
            model.id = 0;
        }

        private void btnAddTarefa_Click(object sender, EventArgs e)
        {
            model.texto = txtDescricao.Text.Trim();
            model.situacao = txtSituacao.Text.Trim();

            using (DBEntities db = new DBEntities())
            {
                if (model.id == 0)
                    db.TAREFAS.Add(model);
                else
                    db.Entry(model).State = EntityState.Modified;
                
                db.SaveChanges();
            }

            Limpar();
            preencherDataGridView();
            MessageBox.Show("Atualizado!");
        }

        void preencherDataGridView()
        {
            using (DBEntities db = new DBEntities())
            {
                dgvListaTarefas.DataSource = db.TAREFAS.ToList<TAREFA>();
            }
        }

        private void dgvListaTarefas_DoubleClick(object sender, EventArgs e)
        {
            if(dgvListaTarefas.CurrentRow.Index != -1)
            {
                model.id = Convert.ToInt32(dgvListaTarefas.CurrentRow.Cells["id"].Value);

                using (DBEntities db = new DBEntities())
                {
                    model = db.TAREFAS.Where(x => x.id == model.id).FirstOrDefault();
                    txtDescricao.Text = model.texto;
                    txtSituacao.Text = model.situacao;
                }

                btnExcluir.Enabled = true;
                btnAddTarefa.Text = "Alterar";
                label3.Text = "Nova Descrição";
            }
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Quer mesmo deletar essa tarefa?", "TODO App", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                using (DBEntities db = new DBEntities())
                {
                    var entry = db.Entry(model);
                    if (entry.State == EntityState.Detached)
                        db.TAREFAS.Attach(model);

                    db.TAREFAS.Remove(model);
                    db.SaveChanges();
                    preencherDataGridView();
                    Limpar();
                    MessageBox.Show("Deletado!");
                }
            }
        }
    }
}
