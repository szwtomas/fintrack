CREATE TABLE ticker_price (
    ticker_price_id SERIAL PRIMARY KEY,
    ticker          VARCHAR(63)               NOT NULL,
    price           DOUBLE PRECISION          NOT NULL,
    currency        VARCHAR(63)               NOT NULL,
    timestamp       TIMESTAMPTZ DEFAULT NOW() NOT NULL
);
