CREATE TABLE holding_group_agg_daily (
    holding_group_agg_daily_id INT PRIMARY KEY  NOT NULL,
    holding_group_id           INT              NOT NULL,
    value                      DOUBLE PRECISION NOT NULL,
    currency                   VARCHAR(63)      NOT NULL,
    date                       TIMESTAMPTZ      NOT NULL,
    FOREIGN KEY (holding_group_id) REFERENCES "holding_group" (holding_group_id) ON DELETE CASCADE
);

CREATE INDEX idx_holding_group_agg_daily_holding_group_id ON holding_group_agg_daily (holding_group_id);
