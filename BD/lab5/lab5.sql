

-- 6.	���������� ��� ���������������� ������ �� ���� ��������
-- ��� ������ ��������� �������������� �������� � ������������ � ����� ���� ������������ ��� ������������� � ������� �������������� � ���������������� �������.
SELECT TABLE_NAME, COLUMN_NAME, DATA_TYPE
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_SCHEMA = 'dbo'

-- 7.	���������� SRID - ������������� ������� ���������
-- ������� SRID �������� SRID 4326, ������� ���������� ������� ��������� WGS 84 (������/�������), ������� ������������ ��� ���-���� � �������� Web Mercator.
-- geometry_columns - ��� edit top 200 - SRID - 4326

SELECT
	srid
FROM
    dbo.geometry_columns

-- 8.	���������� ������������ �������
-- �������� ���������� �� ��������� (���������������) �������������� ��������
SELECT COLUMN_NAME, DATA_TYPE
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_SCHEMA = 'dbo' AND DATA_TYPE <> 'geometry'

-- 9.	������� �������� ���������������� �������� � ������� WKT
--  ��������� ������ ��� ������������� �������������� �������� � ������������
SELECT geom.STAsText() AS WKT_Description
FROM dbo.ne_110m_geography_regions_polys

-- 10.1.	���������� ����������� ���������������� ��������;
-- ����������� �������, ����� ��� ���� ��� ����� ���������������� ��������.
SELECT obj1.geom.STIntersection(obj2.geom) AS Intersection
FROM ne_110m_geography_regions_polys obj1, dbo.ne_110m_geography_regions_polys obj2
WHERE obj1.qgs_fid = 1 AND obj2.qgs_fid = 2

-- 10.2.	���������� ����������� ���������������� ��������;
SELECT obj1.geom.STUnion(obj2.geom) AS [Union]
FROM dbo.ne_110m_geography_regions_polys obj1, dbo.ne_110m_geography_regions_polys obj2
WHERE obj1.qgs_fid = 3 AND obj2.qgs_fid = 2

-- 10.3.	���������� ����������� ���������������� ��������;
SELECT obj1.geom.STWithin(obj2.geom) AS [IsWithin]
FROM dbo.ne_110m_geography_regions_polys obj1, dbo.ne_110m_geography_regions_polys obj2
WHERE obj1.qgs_fid = 1 AND obj2.qgs_fid = 2

-- 10.4.	��������� ����������������� �������;
-- ���������� ��������� ��������� ������� � ����������� ��� �������� �������������.
SELECT geom.Reduce(1) AS Simplified --��������� ������ ������ ���� ������� �������������� ������
FROM dbo.ne_110m_geography_regions_polys
WHERE qgs_fid = 6

-- 10.5.	���������� ��������� ������ ����������������� ��������
SELECT geom.STPointN(1).ToString() AS VertexCoordinates
FROM dbo.ne_110m_geography_regions_polys
WHERE qgs_fid = 6

-- 10.6.	���������� ����������� ���������������� ��������
-- ��������� ����������� ��������������� �������. ��������, ����� ����� ����������� 0, ����� - 1, � ������� - 2.
SELECT geom.STDimension() AS ObjectDimension
FROM dbo.ne_110m_geography_regions_polys
WHERE qgs_fid = 3

-- 10.7.	���������� ����� � ������� ���������������� ��������;
-- ����� (Length): ��������� ����� �������� ��������, ����� ��� ����� ��� ���������.
-- ������� (Area): ��������� ������� ��������� ��������, ����� ��� ��������.
SELECT geom.STLength() AS ObjectLength, geom.STArea() AS ObjectArea
FROM dbo.ne_110m_geography_regions_polys
WHERE qgs_fid = 5


-- 10.8.	���������� ���������� ����� ����������������� ���������;
SELECT obj1.geom.STDistance(obj2.geom) AS Distance
FROM dbo.ne_110m_geography_regions_polys obj1, dbo.ne_110m_geography_regions_polys obj2
WHERE obj1.qgs_fid = 6 AND obj2.qgs_fid = 4


-- 11.	�������� ���������������� ������ � ���� ����� (1) /����� (2) /�������� (3).
-- �����
DECLARE @pointGeometry GEOMETRY;
SET @pointGeometry = GEOMETRY::STGeomFromText('POINT(10 10)', 0);
SELECT @pointGeometry AS PointGeometry;

-- �����
DECLARE @lineGeometry GEOMETRY;
SET @lineGeometry = GEOMETRY::STGeomFromText('LINESTRING(20 5, 5 20, 25 25)', 0);
SELECT @lineGeometry AS LineGeometry;


-- �������
DECLARE @polygonGeometry GEOMETRY;
SET @polygonGeometry = GEOMETRY::STGeomFromText('POLYGON((15 10, 55 55, 5 4, 12 2, 15 10))', 0);
SELECT @polygonGeometry AS PolygonGeometry;


-- 12.	�������, � ����� ���������������� ������� �������� ��������� ���� �������

-- ����� � �������
DECLARE @point GEOMETRY = GEOMETRY::STGeomFromText('POINT(10 10)', 0);
DECLARE @polygon GEOMETRY = GEOMETRY::STGeomFromText('POLYGON((20 20, 20 40, 40 40, 40 20, 20 20))', 0);
SELECT @polygon AS PolygonGeometry;
SELECT @point.STWithin(@polygon) AS PointWithinPolygon;

-- ������ � �������
DECLARE @line GEOMETRY = GEOMETRY::STGeomFromText('LINESTRING(20 5, 5 20, 25 25)', 0);
DECLARE @polygonn GEOMETRY = GEOMETRY::STGeomFromText('POLYGON((20 20, 20 40, 40 40, 40 20, 20 20))', 0);
SELECT @line.STIntersects(@polygonn) AS LineIntersectsPolygon;
