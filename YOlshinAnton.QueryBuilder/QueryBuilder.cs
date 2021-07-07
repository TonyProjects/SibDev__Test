using System;
using System.Collections.Generic;
using System.Text;

namespace YOlshinAnton.QueryBuilder
{
    #region database

    /// <summary>
    /// Таблица базы данных
    /// </summary>
    public class Table : IComparable
    {
        public int Id { get; set; }

        public int Order { get; set; }

        public string Name { get; set; }

        public string Shortcut { get; set; }

        public int CompareTo(object obj)
        {
            return Id.CompareTo(((Table)obj).Id);
        }
    }

    /// <summary>
    /// Столбец Таблицы базы данных
    /// </summary>
    public class TableColumn : IComparable
    {
        public int Id { get; set; }

        public int TableId { get; set; }

        public string Name { get; set; }

        public int CompareTo(object obj)
        {
            return Id.CompareTo(((TableColumn)obj).Id);
        }
    }

    public class TableTableColumn : Table
    {
        public TableTableColumn(Table table)
        {
            Id = table.Id;
            Name = table.Name;
            Order = table.Order;
            Shortcut = table.Shortcut;
        }

        public TableColumn[] Columns { get; set; }
    }

    /// <summary>
    /// Join связь между парой таблиц базы данных
    /// </summary>
    public class DatabaseGraphEdge
    {
        public int Id { get; set; }

        public int Table1 { get; set; }

        public int Table2 { get; set; }

        public string JoinCondition { get; set; }

    }


    /// <summary>
    /// Интерфейс базы данных. Взаимодействие с базой данных
    /// </summary>
    public static class Database
    {
        public static class Generic
        {
            public static T[] GetByIds<T>(List<int> ids, Func<T[]> GetAll)
            {
                T[] all = GetAll();
                T[] select = new T[ids.Count];

                for (int i = 0; i < ids.Count; ++i)

                    select[i] = all[ids[i]];

                return select;
            }
        }

        public static class Repository
        {
            #region External Interface

            /// <summary>
            /// REQUIRED
            /// </summary>
            public static Func<Table[]> SelectTables = GetTables;

            /// <summary>
            /// <dependency>SelectTables</dependency>
            /// </summary>
            public static Func<List<int>, Table[]> SelectTablesByIds = GetTables;

            /// <summary>
            /// <dependency>SelectTablesByIds</dependency>
            /// </summary>
            public static Func<TableColumn[], Table[]> SelectTablesByColumns = GetTables;

            /// <summary>
            /// <dependency>SelectTablesByColumns</dependency>
            /// </summary>
            public static Func<TableColumn[], List<int>> SelectTableIdsByColumns = GetTableIds;

            /// <summary>
            /// <dependency>SelectTablesByIds</dependency>
            /// </summary>
            public static Func<List<int>, List<string>> SelectTableNamesByIds = GetTableNames;


            /// <summary>
            /// REQUIRED
            /// </summary>
            public static Func<TableColumn[]> SelectTableColumns = GetTableColumns;

            /// <summary>
            /// <dependency>SelectTableColumns</dependency>
            /// </summary>
            public static Func<List<int>, TableColumn[]> SelectTableColumnsByIds = GetTableColumns;


            /// <summary>
            /// <dependency>SelectTables</dependency>
            /// <dependency>SelectTableColumns</dependency>
            /// </summary>
            public static Func<TableTableColumn[]> SelectTableTableColumns = GetJoinTablesTableColumns;

            /// <summary>
            /// <dependency>SelectTablesByColumns</dependency>
            /// </summary>
            public static Func<TableColumn[], TableTableColumn[]> SelectTableTableColumnsByColumns = GetJoinTablesTableColumns;


            /// <summary>
            /// REQUIRED
            /// </summary>
            public static Func<DatabaseGraphEdge[]> SelectDatabaseGraphEdges = GetDatabaseGraphEdges;

            #endregion


            // Table
            public static Table[] GetTables()
            {
                return new Table[]
                {
                new Table() { Id=0, Order=0, Name="Alert", Shortcut="A" },
                new Table() { Id=1, Order=1, Name="Mission", Shortcut="M" },
                new Table() { Id=2, Order=2, Name="Document", Shortcut="D" },
                new Table() { Id=3, Order=3, Name="Loan", Shortcut="L" },
                new Table() { Id=4, Order=4, Name="Entity", Shortcut="E"},
                new Table() { Id=5, Order=5, Name="EntitySubType", Shortcut="Est"},
                new Table() { Id=6, Order=6, Name="EntiytType", Shortcut="Et" },
                new Table() { Id=7, Order=7, Name="Negative", Shortcut="Neg" },
                new Table() { Id=8, Order=8, Name="Person", Shortcut="P" },
                new Table() { Id=9, Order=8, Name="Legal", Shortcut="Lgl"},
                };
            }

            public static Table[] GetTables(List<int> indexes)
            {
                return Generic.GetByIds(indexes, SelectTables);
            }

            public static Table[] GetTables(TableColumn[] columns)
            {
                Array.Sort(columns);

                int currentTableId = columns[0].TableId;

                List<int> tableIds = new List<int>();
                tableIds.Add(currentTableId);

                foreach (TableColumn column in columns)
                {
                    if (currentTableId != column.TableId)
                    {
                        currentTableId = column.TableId;
                        tableIds.Add(currentTableId);
                    }
                }

                return SelectTablesByIds(tableIds);
            }

            public static List<int> GetTableIds(TableColumn[] columns)
            {
                var tableIds = new List<int>();

                var tables = SelectTablesByColumns(columns);

                foreach (var table in tables) tableIds.Add(table.Id);

                return tableIds;
            }

            public static List<string> GetTableNames(List<int> indexes)
            {
                var names = new List<string>(indexes.Count);

                var tables = SelectTablesByIds(indexes);

                foreach (var table in tables) names.Add(table.Name);

                return names;
            }


            // TableColumn
            public static TableColumn[] GetTableColumns()
            {
                return new TableColumn[]
                {
                // Alert
                new TableColumn() { Id=0, TableId=0, Name="Id" },
                // Mission
                new TableColumn() { Id=1, TableId=1, Name="Id" },
                // Document
                new TableColumn() { Id=2, TableId=2, Name="Id" },
                new TableColumn() { Id=3, TableId=2, Name="FileName" },
                // Loan
                new TableColumn() { Id=4, TableId=3, Name="Id" },
                // Entity
                new TableColumn() { Id=5, TableId=4, Name="Id" },
                new TableColumn() { Id=6, TableId=4, Name="EntityTypeId" },
                new TableColumn() { Id=7, TableId=4, Name="EntitySubTypeId" },
                // EntitySubType
                new TableColumn() { Id=8, TableId=5, Name="Id" },
                // EntityType
                new TableColumn() { Id=9, TableId=6, Name="Id" },
                // Negative
                new TableColumn() { Id=10, TableId=7, Name="Id" },
                // Person
                new TableColumn() { Id=11, TableId=8, Name="Id" },
                new TableColumn() { Id=12, TableId=8, Name="Name" },
                // Legal
                new TableColumn() { Id=13, TableId=9, Name="Id" },
                new TableColumn() { Id=14, TableId=9, Name="Name" }
                };
            }

            public static TableColumn[] GetTableColumns(List<int> indexes)
            {
                return Generic.GetByIds(indexes, SelectTableColumns);
            }


            // Table Join TableColumn (merge)
            public static TableTableColumn[] GetJoinTablesTableColumns()
            {
                var columns = SelectTableColumns();
                var tables = SelectTables();

                Array.Sort(columns);
                Array.Sort(tables);

                var result = new TableTableColumn[tables.Length];

                for (int i = 0, j = 0; i < tables.Length; ++i)
                {
                    result[i] = new TableTableColumn(tables[i]);

                    var tableColumns = new List<TableColumn>();
                    while (j < columns.Length && result[i].Id == columns[j].TableId)
                    {
                        tableColumns.Add(columns[j++]);
                    }

                    result[i].Columns = tableColumns.ToArray();
                }

                return result;
            }

            public static TableTableColumn[] GetJoinTablesTableColumns(TableColumn[] columns)
            {
                var tables = SelectTablesByColumns(columns);

                Array.Sort(columns, (c1, c2) => {
                    return (c1.TableId < c2.TableId ? -1 : (c1.TableId == c2.TableId ? 0 : 1));
                });
                Array.Sort(tables);

                var result = new TableTableColumn[tables.Length];

                for (int i = 0, j = 0; i < tables.Length; ++i)
                {
                    result[i] = new TableTableColumn(tables[i]);

                    var tableColumns = new List<TableColumn>();
                    while (j < columns.Length && result[i].Id == columns[j].TableId)
                    {
                        tableColumns.Add(columns[j++]);
                    }

                    result[i].Columns = tableColumns.ToArray();
                }

                return result;
            }


            // Graph
            public static Graph<Table> GetDatabaseGraph()
            {
                var t = SelectTables();
                var e = SelectDatabaseGraphEdges();
                var a = GetDatabaseGrahpAdjacent(e);

                return new Graph<Table>(t, a);
            }

            public static DatabaseGraphEdge[] GetDatabaseGraphEdges()
            {
                return new DatabaseGraphEdge[]
                {
                new DatabaseGraphEdge() { Id=0, Table1=0, Table2=1, JoinCondition="left join Mission M on A.Id = M.AlertId" },
                new DatabaseGraphEdge() { Id=1, Table1=0, Table2=2, JoinCondition="left join Document D on A.Id = D.AlertId" },
                new DatabaseGraphEdge() { Id=2, Table1=0, Table2=3, JoinCondition="left join Loan L on A.Id = L.AlertId" },
                new DatabaseGraphEdge() { Id=3, Table1=0, Table2=4, JoinCondition="left join Entity E on A.Id = E.ALertId" },
                new DatabaseGraphEdge() { Id=4, Table1=4, Table2=5, JoinCondition="left join EntitySubType Est on E.EntitySubTypeId = Est.Id" },
                new DatabaseGraphEdge() { Id=5, Table1=5, Table2=6, JoinCondition="left join EntityType Et on Est.EntityTypeId = Et.Id" },
                new DatabaseGraphEdge() { Id=6, Table1=4, Table2=7, JoinCondition="left join Negative Neg on E.Id = Neg.EntityId" },
                new DatabaseGraphEdge() { Id=7, Table1=4, Table2=8, JoinCondition="left join Person P on E.ObjectId = P.Id" },
                new DatabaseGraphEdge() { Id=8, Table1=4, Table2=9, JoinCondition="left join Legal Lgl on E.ObjectId = Lgl.Id" }
                };
            }

            public static int[,] GetDatabaseGrahpAdjacent(DatabaseGraphEdge[] edges)
            {
                var table_count = edges.Length + 1;
                var adjacent = new int[table_count, table_count];

                for (int i = 0; i < table_count - 1; i++)
                {
                    adjacent[edges[i].Table1, edges[i].Table2] = 1;
                    adjacent[edges[i].Table2, edges[i].Table1] = 1;
                }

                return adjacent;
            }
        }

    }

    #endregion


    #region Elements

    /// <summary>
    /// Вершина графа
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Vertex<T>
    {
        public int Index { get; set; }
        public T Value { get; set; }

        public bool Equals(Vertex<T> other)
        {
            return Index.Equals(other.Index) && Value.Equals(other.Value);
        }
    }

    /// <summary>
    /// Ребро графа - Пара вершин, или любая пара
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Edge<T>
    {
        public T A { get; set; }
        public T B { get; set; }

        public bool Equals(Edge<T> other)
        {
            return (A.Equals(other.A) && B.Equals(other.B))
                || (A.Equals(other.B) && B.Equals(other.A));
        }
    }

    /// <summary>
    /// Граф
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Graph<T>
    {
        private int[,] adj;
        private T[,] edges;
        private T[] vertexes;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vertexes">набор вершин</param>
        /// <param name="adj">смежность вершин</param>
        public Graph(T[] vertexes, int[,] adj)
        {
            this.adj = adj;
            this.vertexes = vertexes;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vertexes">набор вершин</param>
        /// <param name="edges">набор пар вершин, граней</param>
        /// <param name="adj">смежность вершин</param>
        public Graph(T[] vertexes, T[,] edges, int[,] adj)
        {
            this.adj = adj;
            this.vertexes = vertexes;
            this.edges = edges;
        }

        public int[,] Adj { get => adj; }
        public T[,] Edges { get => edges; }
        public T[] Vertexes { get => vertexes; }
        public int VertexCount { get => vertexes.Length; }
    }


    /// <summary>
    /// Алгоритмы на графе
    /// </summary>
    public static class GraphAlgorithms
    {
        /// <summary>
        /// Поиск в глубину
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="G"></param>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public static Vertex<T> DepthSearch<T>(Graph<T> G, T A, T B)
        {
            var queue = new Queue<Vertex<T>>();
            var visites = new List<T>() { Capacity = G.VertexCount };

            // Поиск исходной вершины 
            for (int i = 0; i < G.VertexCount; ++i)

                if (A.Equals(G.Vertexes[i])) queue.Enqueue(new Vertex<T>() { Index = i, Value = G.Vertexes[i] });

            // Обход графа
            while (queue.Count != 0)
            {
                var v = queue.Dequeue();

                for (int i = 0; i < G.VertexCount; ++i)
                {
                    var vertex = G.Vertexes[i];
                    var vertexIsAdjacentAndNotVisited = G.Adj[v.Index, i] == 1 && !visites.Contains(vertex);
                    if (vertexIsAdjacentAndNotVisited)
                    {
                        if (B.Equals(vertex)) return new Vertex<T>() { Index = i, Value = vertex };

                        queue.Enqueue(new Vertex<T>() { Index = i, Value = vertex });

                        visites.Add(vertex);
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Поиск в глубину по индексу вершин
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="G"></param>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public static int DepthSearch<T>(Graph<T> G, int A, int B)
        {
            var queue = new Queue<int>();
            var visited = new bool[G.VertexCount];

            // Поиск исходной вершины 
            queue.Enqueue(A);
            visited[A] = true;

            // Обход графа
            while (queue.Count != 0)
            {
                var v = queue.Dequeue();

                for (int vertex = 0; vertex < G.VertexCount; ++vertex)
                {
                    var vertexIsAdjacentAndNotVisited = G.Adj[v, vertex] == 1 && !visited[vertex];
                    if (vertexIsAdjacentAndNotVisited)
                    {
                        if (B == vertex) return vertex;

                        queue.Enqueue(vertex);
                        visited[vertex] = true;
                    }
                }
            }

            return -1;
        }


        /// <summary>
        /// Поиск в ширину
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="G"></param>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public static Vertex<T> WidthSearch<T>(Graph<T> G, T A, T B)
        {
            var queue = new Stack<Vertex<T>>();
            var visites = new List<T>() { Capacity = G.VertexCount };

            // Поиск исходной вершины 
            for (int i = 0; i < G.VertexCount; ++i)

                if (A.Equals(G.Vertexes[i])) queue.Push(new Vertex<T>() { Index = i, Value = G.Vertexes[i] });

            // Обход графа
            while (queue.Count != 0)
            {
                var v = queue.Pop();

                for (int i = 0; i < G.VertexCount; ++i)
                {
                    var vertex = G.Vertexes[i];
                    var vertexIsAdjacentAndNotVisited = G.Adj[v.Index, i] == 1 && !visites.Contains(vertex);
                    if (vertexIsAdjacentAndNotVisited)
                    {
                        if (B.Equals(vertex)) return new Vertex<T>() { Index = i, Value = vertex };

                        queue.Push(new Vertex<T>() { Index = i, Value = vertex });

                        visites.Add(vertex);
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Поиск в ширину по индексу вершин
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="G"></param>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public static int WidthSearch<T>(Graph<T> G, int A, int B)
        {
            var queue = new Stack<int>();
            var visited = new bool[G.VertexCount];

            // Поиск исходной вершины 
            queue.Push(A);
            visited[A] = true;

            // Обход графа
            while (queue.Count != 0)
            {
                var v = queue.Pop();

                for (int vertex = 0; vertex < G.VertexCount; ++vertex)
                {
                    var vertexIsAdjacentAndNotVisited = G.Adj[v, vertex] == 1 && !visited[vertex];
                    if (vertexIsAdjacentAndNotVisited)
                    {
                        if (B == vertex) return vertex;

                        queue.Push(vertex);
                        visited[vertex] = true;
                    }
                }
            }

            return -1;
        }

        /// <summary>
        /// Трасировка искомого пути над алгоритмом "поиска в ширину по индексу вершин
        /// </summary>
        /// <typeparam name="T">Тип вершин графа</typeparam>
        /// <param name="G">
        ///     Граф представляющий базу данных, 
        ///     где порядок вершин и матрицы смежности совпадают
        /// </param>
        /// <param name="A">Исхоная вершина искомого пути</param>
        /// <param name="B">Окончане искомого пути</param>
        /// <returns>Набор индексов вершин представляющий путь из A в B</returns>
        public static List<int> WidthTrace<T>(Graph<T> G, int A, int B)
        {
            var queue = new Queue<int>();
            var visited = new bool[G.VertexCount];

            queue.Enqueue(A);
            visited[A] = true;

            var tracer = new RouteUtil.RouteTracer(G.VertexCount, B);
            tracer.TraceSource(A);

            while (queue.Count != 0)
            {
                // Текущая вершина
                var v = queue.Dequeue();

                // Обход и трасировка смежных ей вершин
                for (int vertex = 0; vertex < G.VertexCount; ++vertex)
                {
                    var vertexIsAdjacentAndNotVisited = G.Adj[v, vertex] == 1 && !visited[vertex];
                    if (vertexIsAdjacentAndNotVisited)
                    {
                        queue.Enqueue(vertex);
                        visited[vertex] = true;

                        if (tracer.TraceForward(v, vertex))
                            return tracer.GetRoute(vertex);
                    }
                }
            }

            return null;
        }
    }


    /// <summary>
    /// 
    /// </summary>
    public static class RouteUtil
    {

        /// <summary>
        /// Обходя граф по алгоритму поиска в ширину, трасирует путь между посещенными вершинами 
        /// с целью найти указанный путь.
        /// 
        /// ИСПОЛЬЗОВАНИЕ
        /// 1   Инициализируется Объект Класса количеством вершин в графе и индексом вершины, 
        ///     что есть окончание искомого пути
        /// 
        /// 2   Перед обходом графа трасируем исходную вершину A вызовом TraceSource(A).
        ///     Если иходная вершна есть окончание искомого пути,
        ///     То   путь найден - путь из единственной исходной вершины.
        ///     
        /// 3   Трасируем смежные вершины A и V методом TraceForward(A, V).
        ///     Если вершина V есть окончание искомого пути,
        ///     То   путь найден
        ///     
        /// 4   Извлекаем искомый путь методом GetRoute(V),
        ///     Где V - окончание искомого пути
        /// </summary>
        public class RouteTracer
        {
            private int F;
            private List<int>[] routes;

            public RouteTracer(int N, int F)
            {
                routes = new List<int>[N];
                this.F = F;
            }

            public bool TraceSource(int A)
            {
                if (routes[A] == null) routes[A] = new List<int>() { A };

                return F == A;
            }

            public bool TraceForward(int A, int V)
            {
                if (routes[A] == null) routes[V] = new List<int>() { A, V };

                else
                if (routes[V] == null) (routes[V] = new List<int>(routes[A])).Add(V);

                return F == V;
            }

            public List<int> GetRoute(int V)
            {
                return routes[V];
            }
        }

        /// <summary>
        /// Для каждой пары соседних вершин из набора вершин графа создается Объект класа Edge
        /// из которых составляется списоко ребер графа. Точки в отрезки.
        /// 
        /// Edges <- (v1, v2) ForAll(v1,v2 in Vertexes) That v1 AdjacentTo v2   
        /// </summary>
        /// <param name="vertexes"></param>
        /// <returns></returns>
        public static List<Edge<int>> VertexesToEdges(List<int> vertexes)
        {
            var edges = new List<Edge<int>>(vertexes.Count - 1);

            for (int i = 0; i < vertexes.Count - 1; i++)

                edges.Add(new Edge<int>() { A = vertexes[i], B = vertexes[i + 1] });

            return edges;
        }

        /// <summary>
        /// Объединение двух путей в графе, представленных наборами ребер
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public static List<Edge<int>> Union(List<Edge<int>> A, List<Edge<int>> B)
        {
            foreach (var b in B)
            {
                bool contained = false;
                foreach (var a in A)
                    if (a.Equals(b)) contained = true;

                if (!contained) A.Add(b);

                contained = false;
            }

            return A;
        }
    }

    #endregion


    /// <summary>
    /// Требование к базе данных:
    /// 1   Недолжно быть циклов;
    /// 
    /// 2   База данных должна иметь структуру дерева, 
    ///     т.е. в ней должен быть корневой элемент 
    ///     в который включены все остальные.
    /// 
    /// Перед использованием необходимо определить:
    /// 1   Справочник Таблиц базы данных и справочник столбцов таблиц;
    /// 
    /// 2   Справочник Join-связей между таблицами базы данных;
    /// 
    /// 3   Удовлетворить сответствие Всех справочников соответствующими Классам:
    ///     Table,
    ///     TableColumn,
    ///     DatabaseGraphEdges;
    ///     
    /// 4   Инициализировать как минимум 3 обязательных делегата класса Database.Repository:
    ///     Database.Repository.SelectTables,
    ///     Database.Repository.SelectTableColumns,
    ///     Database.Repository.SelectDatabaseGraphEdges.
    /// </summary>
    public static class QueryBuilder
    {
        /// <summary>
        /// Строит SQL-запрос по переданному списку идентификаторов столбцов и предикатов.
        /// </summary>
        /// <param name="columnIds">(select) Идентификаторы столбцов </param>
        /// <param name="predicats">(where)  Предикаты </param>
        /// <returns></returns>
        public static string BuildQuery(List<int> columnIds, List<string> predicats=null)
        {
            var queryBuilder = new StringBuilder();

            var dbGraph = Database.Repository.GetDatabaseGraph();
            var dbGraphEdges = Database.Repository.SelectDatabaseGraphEdges();

            var queryColumns = Database.Repository.SelectTableColumnsByIds(columnIds);
            var queryTableIds = Database.Repository.SelectTableIdsByColumns(queryColumns);
            var queryTablesTableColumns = Database.Repository.SelectTableTableColumnsByColumns(queryColumns);

            queryBuilder.Append(BuildSelect(queryTablesTableColumns));

            queryBuilder.Append(BuildFrom(queryTableIds, dbGraph, dbGraphEdges));

            if (predicats != null)
            queryBuilder.Append(BuildWhere(predicats));

            return queryBuilder.ToString();
        }


        /// <summary>
        /// Строит Select-сегмент
        /// </summary>
        public static string BuildSelect(TableTableColumn[] tablesTableColumns)
        {
            var selectSegmentBuilder = new StringBuilder("select distinct ");

            foreach (var table in tablesTableColumns)
                foreach (var column in table.Columns)
                {
                    selectSegmentBuilder.AppendFormat("{0}.{1},", table.Shortcut, column.Name);
                }

            selectSegmentBuilder.Replace(',', '\n', selectSegmentBuilder.Length - 1, 1);


            return selectSegmentBuilder.ToString();
        }


        static private string EdgesToJoins(List<Edge<int>> routeEdges, DatabaseGraphEdge[] graphEdges)
        {
            var joinSegmentBuilder = new StringBuilder();


            foreach (var edge in routeEdges)
            {
                foreach (var gedge in graphEdges)
                {
                    if ((edge.A == gedge.Table1 && edge.B == gedge.Table2)
                    || (edge.A == gedge.Table2 && edge.B == gedge.Table1))
                    {
                        joinSegmentBuilder.Append(gedge.JoinCondition);
                        joinSegmentBuilder.Append('\n');
                    }
                }
            }

            return joinSegmentBuilder.ToString();
        }

        /// <summary>
        /// Строит From-сегмент
        /// </summary>
        /// <param name="G">Граф представляющий базу данных</param>
        public static string BuildFrom(List<int> queryTableIndexes, Graph<Table> G, DatabaseGraphEdge[] graphEdges)
        {
            // Искомый маршрут в виде ребер графа
            var routeEdges = new List<Edge<int>>();

            // Пары из соседних вершин 
            var queryTableIndexPairs = RouteUtil.VertexesToEdges(queryTableIndexes);

            // Объединение найденных путей в граффе, заданных парой вершнин
            foreach (var pair in queryTableIndexPairs)

                RouteUtil.Union(routeEdges, RouteUtil.VertexesToEdges(GraphAlgorithms.WidthTrace(G, pair.A, pair.B)));


            // Строим сегмент From
            int minIndex = queryTableIndexes[0];

            foreach (var edge in routeEdges)

                if (minIndex > edge.A || minIndex > edge.B)
                    minIndex = edge.A < edge.B ? edge.A : edge.B;

            var fromSegmentBuilder = new StringBuilder("from ");

            fromSegmentBuilder.AppendFormat("{0} {1}\n", G.Vertexes[minIndex].Name,
                                                         G.Vertexes[minIndex].Shortcut);
            fromSegmentBuilder.Append(EdgesToJoins(routeEdges, graphEdges));

            return fromSegmentBuilder.ToString();
        }


        /// <summary>
        /// Строит Where-сегмент
        /// </summary>
        public static string BuildWhere(List<string> predicats)
        {
            if (predicats == null || predicats.Count == 0)
                return "";

            var whereSegmentBuilder = new StringBuilder("where ");

            for (int i = 0; i < predicats.Count - 1; ++i)
            {
                whereSegmentBuilder.Append(predicats[i]);
                whereSegmentBuilder.Append(" and ");
            }
            whereSegmentBuilder.Append(predicats[predicats.Count - 1]);
            whereSegmentBuilder.Append('\n');

            return whereSegmentBuilder.ToString();
        }
    }

}
