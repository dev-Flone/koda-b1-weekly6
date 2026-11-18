-- SELECT cc.name AS ConstraintName,
--        cc.definition AS ConstraintDefinition
-- FROM   sys.check_constraints AS cc
--        INNER JOIN
--        sys.tables AS t
--        ON cc.parent_object_id = t.object_id
-- WHERE  t.name = 'Wallets';
-- ALTER TABLE Wallets DROP CONSTRAINT Ck_Wallets_Balance;
-- ALTER TABLE Wallets
--        ADD CONSTRAINT Ck_Wallets_Balance CHECK (Balance >= 0);
-- SELECT COLUMN_NAME,
--        IS_NULLABLE,
--        DATA_TYPE
-- FROM   INFORMATION_SCHEMA.COLUMNS
-- WHERE  TABLE_NAME = 'Transactions'
--        AND COLUMN_NAME = 'Description';
-- ALTER TABLE Transactions ALTER COLUMN Description NVARCHAR (250) NULL;
-- -- Standard ANSI Query
-- SELECT TABLE_NAME,
--        TABLE_TYPE
-- FROM   INFORMATION_SCHEMA.TABLES;
-- -- T-SQL System Views Query
SELECT COUNT(*) AS TotalUsers
FROM   dbo.Users;

SELECT COUNT(*) AS TotalWallets
FROM   dbo.Wallets;

SELECT name AS ObjectName,
       type_desc AS ObjectType
FROM   sys.objects
WHERE  type IN ('U', 'V'); -- 'U' = User Table, 'V' = View

SELECT table_name
FROM   information_schema.tables
WHERE  table_type = 'BASE TABLE';

DBCC CHECKIDENT ('Users', RESEED, 0);

DELETE Users;

DBCC CHECKIDENT ('Wallets', RESEED, 0);

DELETE Wallets;

SELECT *
FROM   Users;

SELECT *
FROM   Wallets;

SELECT *
FROM   Transactions;

SELECT *
FROM   WalletMutations;

SELECT *
FROM   PaymentMethods;

SELECT *
FROM   TopUpRequests;

SELECT *
FROM   AuditLogs;

SELECT (SELECT COUNT(*)
        FROM   Transactions) AS TotalTransactions,
       (SELECT COUNT(*)
        FROM   AuditLogs) AS TotalAuditLogs;

SELECT ReferenceNo,
       Id,
       Status
FROM   Transactions
WHERE  ReferenceNo LIKE 'SEED-%';

-- Reset DATABASE
BEGIN TRANSACTION;

DELETE AuditLogs;

DELETE WalletMutations;

DELETE TopUpRequests;

DELETE Transactions;

DELETE Wallets;

DELETE PaymentMethods;

DELETE Users;

COMMIT TRANSACTION;

SELECT   Id,
         Code,
         Name,
         IsActive
FROM     PaymentMethods
ORDER BY Id;

SELECT   *
FROM     Transactions
ORDER BY Id DESC;

SELECT   *
FROM     TopUpRequests
ORDER BY Id DESC;

SELECT fk.name AS ForeignKeyName,
       OBJECT_NAME(fk.parent_object_id) AS TableName,
       COL_NAME(fkc.parent_object_id, fkc.parent_column_id) AS ColumnName,
       OBJECT_NAME(fk.referenced_object_id) AS ReferencedTable,
       COL_NAME(fkc.referenced_object_id, fkc.referenced_column_id) AS ReferencedColumn
FROM   sys.foreign_keys AS fk
       INNER JOIN
       sys.foreign_key_columns AS fkc
       ON fk.object_id = fkc.constraint_object_id
WHERE  OBJECT_NAME(fk.parent_object_id) = 'TopUpRequests';

SELECT COLUMN_NAME,
       DATA_TYPE,
       IS_NULLABLE,
       COLUMNPROPERTY(OBJECT_ID('dbo.PaymentMethods'), COLUMN_NAME, 'IsIdentity') AS IsIdentity
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'PaymentMethods';

SELECT *
FROM   dbo.__EFMigrationsHistory
WHERE  MigrationId LIKE '%UpdatePaymentMethodFixedIds%';

SELECT   *
FROM     dbo.PaymentMethods
ORDER BY Id;

SELECT   Id,
         TransactionId,
         WalletId,
         PaymentMethodId,
         Amount,
         Status
FROM     dbo.TopUpRequests
ORDER BY Id;

SELECT *
FROM   AuditLogs
WHERE  Action = 'TRANSFER';

SELECT   Action,
         COUNT(*) AS Total
FROM     AuditLogs
GROUP BY Action;

SELECT   Id,
         Action,
         Type,
         Outcome,
         Description,
         TransactionId
FROM     AuditLogs
ORDER BY Id;

SELECT   Action,
         COUNT(*) AS Total
FROM     AuditLogs
GROUP BY Action;

-- Update Audit log based on new auditType
BEGIN TRANSACTION;

UPDATE AuditLogs
SET    Action = CASE Id WHEN 1 THEN 'TOPUP_PAID' WHEN 2 THEN 'TOPUP_REQUESTED' WHEN 3 THEN 'TOPUP_REQUESTED' WHEN 4 THEN 'TRANSFER_CREATED' WHEN 5 THEN 'TRANSFER_REJECTED' END
WHERE  Id IN (1, 2, 3, 4, 5);

SELECT   Id,
         Action,
         Outcome,
         Description
FROM     AuditLogs
ORDER BY Id;

COMMIT TRANSACTION;

SELECT   Action,
         COUNT(*) AS Total
FROM     dbo.AuditLogs
GROUP BY Action;