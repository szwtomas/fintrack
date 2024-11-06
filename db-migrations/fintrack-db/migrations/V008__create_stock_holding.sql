CREATE TABLE stock_holding (
    holding_id  INT PRIMARY KEY           NOT NULL,
    amount      INT                       NOT NULL,
    ticker      VARCHAR(63)               NOT NULL,
    created_at  TIMESTAMPTZ DEFAULT NOW() NOT NULL,
    updated_at  TIMESTAMPTZ DEFAULT NOW() NOT NULL,
    FOREIGN KEY (holding_id) REFERENCES "holding" (holding_id) ON DELETE CASCADE
);

CREATE OR REPLACE FUNCTION update_updated_at_column_stock_holding()
    RETURNS TRIGGER AS
$$
BEGIN
    NEW.updated_at = NOW();
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER update__holding_updated_at
    BEFORE UPDATE
    ON stock_holding
    FOR EACH ROW
EXECUTE FUNCTION update_updated_at_column_stock_holding();
