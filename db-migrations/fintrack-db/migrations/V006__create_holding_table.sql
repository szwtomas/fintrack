CREATE TABLE holding (
    holding_id       SERIAL PRIMARY KEY,
    holding_group_id INTEGER            NOT NULL,
    type             VARCHAR(128)       NOT NULL,
    created_at       TIMESTAMPTZ DEFAULT NOW() NOT NULL,
    updated_at       TIMESTAMPTZ DEFAULT NOW() NOT NULL,
    FOREIGN KEY (holding_group_id) REFERENCES "holding_group" (holding_group_id) ON DELETE CASCADE
);

CREATE OR REPLACE FUNCTION update_updated_at_column_holding()
    RETURNS TRIGGER AS
$$
BEGIN
    NEW.updated_at = NOW();
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER update_holding_updated_at
    BEFORE UPDATE
    ON holding
    FOR EACH ROW
EXECUTE FUNCTION update_updated_at_column_holding();