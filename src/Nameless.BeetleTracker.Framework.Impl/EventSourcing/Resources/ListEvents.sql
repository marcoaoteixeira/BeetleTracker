SELECT
    AggregateID,
    Version,
    Payload,
    TimeStamp,
    EventType,
    Owner
FROM Events (NOLOCK)
WHERE
    AggregateID = @AggregateID
AND Version >= @Version
AND Owner = @Owner;