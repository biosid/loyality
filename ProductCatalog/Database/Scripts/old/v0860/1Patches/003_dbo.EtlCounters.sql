UPDATE [dbo].[EtlCounters]
  SET [CounterName] = 'Импортировано без ошибок'
WHERE [EtlPackageId] = '77A3E3C6-C00B-41FF-8376-DCEF0DF79A00'
  AND [CounterName] = 'Импортировано'
  
UPDATE [dbo].[EtlCounters]
  SET [CounterName] = 'Ошибочные (не импортировано)'
WHERE [EtlPackageId] = '77A3E3C6-C00B-41FF-8376-DCEF0DF79A00'
  AND [CounterName] = 'Ошибочные'