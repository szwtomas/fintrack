CREATE TABLE ticker_price (
    ticker VARCHAR(63) NOT NULL,
    currency VARCHAR(63) NOT NULL,
    timestamp TIMESTAMPTZ DEFAULT NOW() NOT NULL
);
