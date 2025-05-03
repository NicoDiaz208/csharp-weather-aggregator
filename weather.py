from meteostat import Point, Daily, Hourly
from datetime import datetime
import pandas as pd

# define Geographical point for linz
linz = Point(48.3069, 14.2858)

# Timerange for Weather Data
start = datetime(2000, 1, 1)
end = datetime(2025, 4, 9)

start_h = pd.to_datetime('2018-10-26 12:00:00.0000000011',
                         format='%Y-%m-%d %H:%M:%S.%f')
end_h = pd.to_datetime('2025-04-14 12:00:00.0000000011',
                       format='%Y-%m-%d %H:%M:%S.%f')

print(end_h)
print(start_h)

# Get hourly data
data_h = Hourly('72219', start_h, end_h)
df_hourly = data_h.fetch()


# Vorschau anzeigen
print(df_hourly.head())

# Als CSV speichern
df_hourly.to_csv('hourly_linz.csv')
