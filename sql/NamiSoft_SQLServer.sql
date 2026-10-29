/* =========================================================================
   NamiSoft - Control de asistencia y nomina semanal
   Esquema para Microsoft SQL Server 2022 (Developer / Express)
   Generado a partir de las migraciones de EF Core 10.0.14.

   Uso en SSMS: abrir este archivo y ejecutar (F5).
   Uso con sqlcmd:
     sqlcmd -S localhost,1433 -U sa -C -N -i NamiSoft_SQLServer.sql
   ========================================================================= */

IF DB_ID(N'NamiSoft') IS NULL
    CREATE DATABASE NamiSoft;
GO

USE NamiSoft;
GO

IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261029065428_InitialCreate'
)
BEGIN
    CREATE TABLE [Checadas] (
        [Id] int NOT NULL IDENTITY,
        [NumeroEmpleado] nvarchar(32) NOT NULL,
        [Nombre] nvarchar(max) NOT NULL,
        [Fecha] datetime2 NOT NULL,
        [PrimeraChecada] time NULL,
        [UltimaChecada] time NULL,
        [HorasExtra] decimal(10,2) NOT NULL,
        [ModificadaManual] bit NOT NULL,
        [HorasTrabajadas] decimal(10,2) NOT NULL,
        [RetardoMinutos] int NOT NULL,
        [TieneRetardo] bit NOT NULL,
        [Origen] int NOT NULL,
        [CreadaEn] datetime2 NOT NULL,
        [ModificadaEn] datetime2 NULL,
        [Motivo] nvarchar(300) NULL,
        CONSTRAINT [PK_Checadas] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261029065428_InitialCreate'
)
BEGIN
    CREATE TABLE [ChecadasPendientes] (
        [Id] int NOT NULL IDENTITY,
        [NumeroEmpleado] nvarchar(32) NOT NULL,
        [Nombre] nvarchar(max) NOT NULL,
        [Fecha] datetime2 NOT NULL,
        [PrimeraChecada] time NULL,
        [UltimaChecada] time NULL,
        [Motivo] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_ChecadasPendientes] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261029065428_InitialCreate'
)
BEGIN
    CREATE TABLE [ConfiguracionSistema] (
        [Id] int NOT NULL IDENTITY,
        [EntradaManana] time NOT NULL,
        [SalidaManana] time NOT NULL,
        [ToleranciaManana] int NOT NULL,
        [EntradaTarde] time NOT NULL,
        [SalidaTarde] time NOT NULL,
        [ToleranciaTarde] int NOT NULL,
        [CadaRetardos] int NOT NULL,
        [DescuentoRetardosMinutos] int NOT NULL,
        [AplicarPrimaDominical] bit NOT NULL,
        [PorcentajePrimaDominical] decimal(5,2) NOT NULL,
        CONSTRAINT [PK_ConfiguracionSistema] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261029065428_InitialCreate'
)
BEGIN
    CREATE TABLE [Empleados] (
        [Id] int NOT NULL IDENTITY,
        [NumeroEmpleado] nvarchar(32) NOT NULL,
        [Nombre] nvarchar(200) NOT NULL,
        [Puesto] nvarchar(120) NOT NULL,
        [Departamento] nvarchar(120) NOT NULL,
        [SalarioHora] decimal(18,2) NOT NULL,
        [Turno] nvarchar(32) NOT NULL,
        [TipoHorario] int NOT NULL,
        [Descanso] nvarchar(32) NOT NULL,
        [Activo] bit NOT NULL,
        CONSTRAINT [PK_Empleados] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261029065428_InitialCreate'
)
BEGIN
    CREATE TABLE [HorariosEmpleado] (
        [Id] int NOT NULL IDENTITY,
        [NumeroEmpleado] nvarchar(32) NOT NULL,
        [DiaSemana] nvarchar(24) NOT NULL,
        [HoraEntrada] time NOT NULL,
        [HoraSalida] time NOT NULL,
        CONSTRAINT [PK_HorariosEmpleado] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261029065428_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Checadas_NumeroEmpleado_Fecha] ON [Checadas] ([NumeroEmpleado], [Fecha]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261029065428_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_ChecadasPendientes_NumeroEmpleado_Fecha] ON [ChecadasPendientes] ([NumeroEmpleado], [Fecha]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261029065428_InitialCreate'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Empleados_NumeroEmpleado] ON [Empleados] ([NumeroEmpleado]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261029065428_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_HorariosEmpleado_NumeroEmpleado_DiaSemana] ON [HorariosEmpleado] ([NumeroEmpleado], [DiaSemana]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261029065428_InitialCreate'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20261029065428_InitialCreate', N'10.0.14');
END;

COMMIT;
GO

/* =========================================================================
   Datos iniciales (idempotente: no duplica si ya existen)
   ========================================================================= */

IF NOT EXISTS (SELECT 1 FROM dbo.ConfiguracionSistema)
BEGIN
    INSERT INTO dbo.ConfiguracionSistema
        (EntradaManana, SalidaManana, ToleranciaManana,
         EntradaTarde, SalidaTarde, ToleranciaTarde,
         CadaRetardos, DescuentoRetardosMinutos,
         AplicarPrimaDominical, PorcentajePrimaDominical)
    VALUES
        ('07:00:00', '17:00:00', 10,
         '12:00:00', '22:00:00', 10,
         3, 30, 1, 25.00);
END;
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Empleados WHERE NumeroEmpleado = N'E-001')
BEGIN
    INSERT INTO dbo.Empleados
        (NumeroEmpleado, Nombre, Puesto, Departamento, SalarioHora, Turno, TipoHorario, Descanso, Activo)
    VALUES
        (N'E-001', N'Ana Lopez Ramirez',  N'Operadora',  N'Produccion', 48.75, N'Matutino', 0, N'Domingo', 1),
        (N'E-002', N'Luis Torres Mena',   N'Almacenista', N'Logistica',  52.30, N'Vespertino', 1, N'Sabado',  1);
END;
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Checadas WHERE NumeroEmpleado = N'E-001' AND Fecha = '2026-10-27')
BEGIN
    INSERT INTO dbo.Checadas
        (NumeroEmpleado, Nombre, Fecha, PrimeraChecada, UltimaChecada,
         HorasExtra, ModificadaManual, HorasTrabajadas, RetardoMinutos,
         TieneRetardo, Origen, CreadaEn, ModificadaEn, Motivo)
    VALUES
        (N'E-001', N'Ana Lopez Ramirez', '2026-10-27', '07:58:00', '17:03:00',
         1.50, 0, 9.08, 0, 0, 0, SYSDATETIME(), NULL, NULL),
        (N'E-002', N'Luis Torres Mena',  '2026-10-27', '13:55:00', '22:05:00',
         0.00, 0, 8.17, 0, 0, 0, SYSDATETIME(), NULL, NULL);
END;
GO

PRINT 'NamiSoft: esquema y datos iniciales aplicados correctamente.';
GO
