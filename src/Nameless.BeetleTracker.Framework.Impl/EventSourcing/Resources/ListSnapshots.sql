SELECT
    AggregateID,
    Version,
    Payload,
    TimeStamp,
    EventType,
    Owner
FROM Snapshots (NOLOCK)
WHERE
    AggregateID = @AggregateID
AND Version >= @Version
AND Owner = @Owner;