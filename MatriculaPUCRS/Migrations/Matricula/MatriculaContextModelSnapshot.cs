﻿// <auto-generated />
using System;
using Infraestrutura.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MatriculaPUCRS.Migrations.Matricula
{
    [DbContext(typeof(MatriculaContext))]
    partial class MatriculaContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.17")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CurriculoDisciplina", b =>
                {
                    b.Property<long>("CurriculosId")
                        .HasColumnType("bigint");

                    b.Property<long>("DisciplinasId")
                        .HasColumnType("bigint");

                    b.HasKey("CurriculosId", "DisciplinasId");

                    b.HasIndex("DisciplinasId");

                    b.ToTable("CurriculoDisciplina");
                });

            modelBuilder.Entity("DisciplinaDisciplina", b =>
                {
                    b.Property<long>("DisciplinaOrigemId")
                        .HasColumnType("bigint");

                    b.Property<long>("RequisitosId")
                        .HasColumnType("bigint");

                    b.HasKey("DisciplinaOrigemId", "RequisitosId");

                    b.HasIndex("RequisitosId");

                    b.ToTable("DisciplinaDisciplina");
                });

            modelBuilder.Entity("Entidades.Modelos.Curriculo", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Ativo")
                        .HasColumnType("bit");

                    b.Property<string>("Codigo")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("NomeDoCurso")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasAlternateKey("Codigo");

                    b.ToTable("Curriculos");
                });

            modelBuilder.Entity("Entidades.Modelos.Disciplina", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CargaHoraria")
                        .HasColumnType("int");

                    b.Property<string>("Codigo")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("Nivel")
                        .HasColumnType("int");

                    b.Property<string>("Nome")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasAlternateKey("Codigo");

                    b.ToTable("Disciplinas");
                });

            modelBuilder.Entity("Entidades.Modelos.Estudante", b =>
                {
                    b.Property<long>("Id")
                        .HasColumnType("bigint");

                    b.Property<string>("CPF")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<long?>("CurriculoId")
                        .HasColumnType("bigint");

                    b.Property<int>("DigitoVerificador")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("int")
                        .HasComputedColumnSql("CONVERT(INT, (Estudantes.Id % 9) + 1)");

                    b.Property<string>("Estado")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasAlternateKey("CPF");

                    b.HasIndex("CurriculoId");

                    b.ToTable("Estudantes");
                });

            modelBuilder.Entity("Entidades.Modelos.HorarioGrade", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Horario")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("HorarioFinal")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HorarioInicial")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasAlternateKey("Horario");

                    b.ToTable("HorariosGrade");
                });

            modelBuilder.Entity("Entidades.Modelos.MatriculaTurma", b =>
                {
                    b.Property<long>("TurmaId")
                        .HasColumnType("bigint");

                    b.Property<long>("EstudanteId")
                        .HasColumnType("bigint");

                    b.Property<string>("Estado")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<float?>("Nota")
                        .HasColumnType("real");

                    b.HasKey("TurmaId", "EstudanteId");

                    b.HasIndex("EstudanteId");

                    b.ToTable("MatriculaTurmas");
                });

            modelBuilder.Entity("Entidades.Modelos.Semestre", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DataFinal")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DataInicial")
                        .HasColumnType("datetime2");

                    b.Property<string>("Titulo")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("Titulo")
                        .IsUnique();

                    b.ToTable("Semestres");
                });

            modelBuilder.Entity("Entidades.Modelos.Turma", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("DisciplinaId")
                        .HasColumnType("bigint");

                    b.Property<int>("NumeroDeVagas")
                        .HasColumnType("int");

                    b.Property<long>("SemestreId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("DisciplinaId");

                    b.HasIndex("SemestreId");

                    b.ToTable("Turmas");
                });

            modelBuilder.Entity("HorarioGradeTurma", b =>
                {
                    b.Property<long>("HorariosId")
                        .HasColumnType("bigint");

                    b.Property<long>("TurmasId")
                        .HasColumnType("bigint");

                    b.HasKey("HorariosId", "TurmasId");

                    b.HasIndex("TurmasId");

                    b.ToTable("HorarioGradeTurma");
                });

            modelBuilder.Entity("CurriculoDisciplina", b =>
                {
                    b.HasOne("Entidades.Modelos.Curriculo", null)
                        .WithMany()
                        .HasForeignKey("CurriculosId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Entidades.Modelos.Disciplina", null)
                        .WithMany()
                        .HasForeignKey("DisciplinasId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DisciplinaDisciplina", b =>
                {
                    b.HasOne("Entidades.Modelos.Disciplina", null)
                        .WithMany()
                        .HasForeignKey("DisciplinaOrigemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Entidades.Modelos.Disciplina", null)
                        .WithMany()
                        .HasForeignKey("RequisitosId")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Entidades.Modelos.Estudante", b =>
                {
                    b.HasOne("Entidades.Modelos.Curriculo", "Curriculo")
                        .WithMany("Estudantes")
                        .HasForeignKey("CurriculoId");

                    b.Navigation("Curriculo");
                });

            modelBuilder.Entity("Entidades.Modelos.MatriculaTurma", b =>
                {
                    b.HasOne("Entidades.Modelos.Estudante", "Estudante")
                        .WithMany("Matriculas")
                        .HasForeignKey("EstudanteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Entidades.Modelos.Turma", "Turma")
                        .WithMany("Matriculas")
                        .HasForeignKey("TurmaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Estudante");

                    b.Navigation("Turma");
                });

            modelBuilder.Entity("Entidades.Modelos.Turma", b =>
                {
                    b.HasOne("Entidades.Modelos.Disciplina", "Disciplina")
                        .WithMany("Turmas")
                        .HasForeignKey("DisciplinaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Entidades.Modelos.Semestre", "Semestre")
                        .WithMany("Turmas")
                        .HasForeignKey("SemestreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Disciplina");

                    b.Navigation("Semestre");
                });

            modelBuilder.Entity("HorarioGradeTurma", b =>
                {
                    b.HasOne("Entidades.Modelos.HorarioGrade", null)
                        .WithMany()
                        .HasForeignKey("HorariosId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Entidades.Modelos.Turma", null)
                        .WithMany()
                        .HasForeignKey("TurmasId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Entidades.Modelos.Curriculo", b =>
                {
                    b.Navigation("Estudantes");
                });

            modelBuilder.Entity("Entidades.Modelos.Disciplina", b =>
                {
                    b.Navigation("Turmas");
                });

            modelBuilder.Entity("Entidades.Modelos.Estudante", b =>
                {
                    b.Navigation("Matriculas");
                });

            modelBuilder.Entity("Entidades.Modelos.Semestre", b =>
                {
                    b.Navigation("Turmas");
                });

            modelBuilder.Entity("Entidades.Modelos.Turma", b =>
                {
                    b.Navigation("Matriculas");
                });
#pragma warning restore 612, 618
        }
    }
}
