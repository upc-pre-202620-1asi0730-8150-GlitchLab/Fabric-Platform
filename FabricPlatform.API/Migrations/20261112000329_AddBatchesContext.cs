using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FabricPlatform.API.Migrations
{
    /// <inheritdoc />
    public partial class AddBatchesContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "alerts",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    status = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    type = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    reference = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    title = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    description = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false),
                    action = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    accumulated = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    limit = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    excess = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    machine = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    batch = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_alerts", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "batches",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    code = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    garment_model = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    status = table.Column<string>(type: "longtext", nullable: false),
                    projected_quantity = table.Column<int>(type: "int", nullable: false),
                    processed_quantity = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    current_stage = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    progress_percentage = table.Column<int>(type: "int", nullable: false),
                    delivery_date = table.Column<DateTimeOffset>(type: "datetime", nullable: false),
                    technical_sheet_url = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true),
                    notes = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_batches", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "defects",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    batch_id = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    defect_type = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    quantity = table.Column<int>(type: "int", nullable: false),
                    machine_id = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    origin = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    status = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    observation = table.Column<string>(type: "longtext", nullable: false),
                    disposition = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    correction_type = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    date = table.Column<string>(type: "varchar(35)", maxLength: 35, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_defects", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "fabric_inspections",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    roll_id = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    supplier = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    inspection_date = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    sample = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    color = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    initial_result = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    roll_status = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    shrinkage_test = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    parameters = table.Column<string>(type: "longtext", nullable: true),
                    parameter_history = table.Column<string>(type: "longtext", nullable: false),
                    test_history = table.Column<string>(type: "longtext", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_fabric_inspections", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "machines",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    code = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    type = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    status = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    location = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    acquired_date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_machines", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "operators",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    Specialty = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_operators", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "batch_movements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    BatchId = table.Column<int>(type: "int", nullable: false),
                    stage_name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    QtyIn = table.Column<int>(type: "int", nullable: false),
                    QtyOut = table.Column<int>(type: "int", nullable: false),
                    difference = table.Column<int>(type: "int", nullable: false),
                    ResponsiblePerson = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    Timestamp = table.Column<DateTimeOffset>(type: "datetime", nullable: false),
                    status = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_batch_movements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_batch_movements_batches_BatchId",
                        column: x => x.BatchId,
                        principalTable: "batches",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "batch_observations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    BatchId = table.Column<int>(type: "int", nullable: false),
                    AuthorName = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    Category = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    Content = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_batch_observations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_batch_observations_batches_BatchId",
                        column: x => x.BatchId,
                        principalTable: "batches",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "batch_operators",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    BatchId = table.Column<int>(type: "int", nullable: false),
                    OperatorId = table.Column<int>(type: "int", nullable: false),
                    FullName = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    Specialty = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    assigned_stage = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    AssignmentDate = table.Column<DateTimeOffset>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_batch_operators", x => x.Id);
                    table.ForeignKey(
                        name: "FK_batch_operators_batches_BatchId",
                        column: x => x.BatchId,
                        principalTable: "batches",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "batch_stages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    BatchId = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    ProcessedQuantity = table.Column<int>(type: "int", nullable: false),
                    TargetQuantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_batch_stages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_batch_stages_batches_BatchId",
                        column: x => x.BatchId,
                        principalTable: "batches",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "machine_downtimes",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    machine_id = table.Column<int>(type: "int", nullable: false),
                    reason = table.Column<string>(type: "varchar(120)", maxLength: 120, nullable: false),
                    start_time = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    end_time = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    duration_minutes = table.Column<int>(type: "int", nullable: true),
                    solution = table.Column<string>(type: "varchar(240)", maxLength: 240, nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_machine_downtimes", x => x.id);
                    table.ForeignKey(
                        name: "f_k_machine_downtimes_machines_machine_id",
                        column: x => x.machine_id,
                        principalTable: "machines",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "maintenance_records",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    machine_id = table.Column<int>(type: "int", nullable: false),
                    technician = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: false),
                    maintenance_type = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    description = table.Column<string>(type: "varchar(240)", maxLength: 240, nullable: false),
                    performed_date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_maintenance_records", x => x.id);
                    table.ForeignKey(
                        name: "f_k_maintenance_records_machines_machine_id",
                        column: x => x.machine_id,
                        principalTable: "machines",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.InsertData(
                table: "operators",
                columns: new[] { "Id", "Name", "Specialty" },
                values: new object[,]
                {
                    { 1, "Carlos Mendoza", "Overlock" },
                    { 2, "Ana Torres", "Straight Stitch" },
                    { 3, "Luis Ramos", "Finishing" },
                    { 4, "Maria Lopez", "Quality Control" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_batch_movements_BatchId",
                table: "batch_movements",
                column: "BatchId");

            migrationBuilder.CreateIndex(
                name: "IX_batch_observations_BatchId",
                table: "batch_observations",
                column: "BatchId");

            migrationBuilder.CreateIndex(
                name: "IX_batch_operators_BatchId",
                table: "batch_operators",
                column: "BatchId");

            migrationBuilder.CreateIndex(
                name: "IX_batch_stages_BatchId",
                table: "batch_stages",
                column: "BatchId");

            migrationBuilder.CreateIndex(
                name: "i_x_machine_downtimes_machine_id",
                table: "machine_downtimes",
                column: "machine_id");

            migrationBuilder.CreateIndex(
                name: "i_x_machines_code",
                table: "machines",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_x_maintenance_records_machine_id",
                table: "maintenance_records",
                column: "machine_id");

            migrationBuilder.CreateIndex(
                name: "IX_operators_Name",
                table: "operators",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "alerts");

            migrationBuilder.DropTable(
                name: "batch_movements");

            migrationBuilder.DropTable(
                name: "batch_observations");

            migrationBuilder.DropTable(
                name: "batch_operators");

            migrationBuilder.DropTable(
                name: "batch_stages");

            migrationBuilder.DropTable(
                name: "defects");

            migrationBuilder.DropTable(
                name: "fabric_inspections");

            migrationBuilder.DropTable(
                name: "machine_downtimes");

            migrationBuilder.DropTable(
                name: "maintenance_records");

            migrationBuilder.DropTable(
                name: "operators");

            migrationBuilder.DropTable(
                name: "batches");

            migrationBuilder.DropTable(
                name: "machines");
        }
    }
}
