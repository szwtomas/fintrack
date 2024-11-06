CREATE TABLE exchange_rate (
    exchange_rate_id  SERIAL PRIMARY KEY,
    exchange          VARCHAR(63)               NOT NULL,
    price             DOUBLE PRECISION          NOT NULL,
    exchange_type    VARCHAR(63),
    timestamp         TIMESTAMPTZ DEFAULT NOW() NOT NULL
);
