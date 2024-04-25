using System.Collections.ObjectModel;
using System.Windows.Media;

namespace AgeenkovApp.Model {
    public class Area : PropChange {
        private int id;
        public int Id {
            get => id;
            set { id = value; OnPropertyChanged(nameof(Id)); }
        }

        private string? name;
        public string? Name {
            get => name;
            set { name = value; OnPropertyChanged(nameof(Name)); }
        }

        private Project project;
        public Project Project {
            get => project;
            set { project = value; OnPropertyChanged(nameof(Project)); }
        }

        private ObservableCollection<AreaCoords> coords;
        public ObservableCollection<AreaCoords> Coords {
            get => coords;
            set { coords = value; OnPropertyChanged(nameof(Coords)); }
        }

        private ObservableCollection<Profile> profiles;
        public ObservableCollection<Profile> Profiles {
            get => profiles;
            set { profiles = value; OnPropertyChanged(nameof(Profiles)); }
        }

        public void Draw(DrawModel dm, Brush br) {
            if(coords is null) return;
            dm.DrawPoly(coords.Select(p => p.AsPoint).ToArray(), br, 0.2, true);
            foreach(var p in coords) {
                dm.DrawText($"{p.X};{p.Y}", p.X, p.Y, br, 1);
                dm.DrawCircle(p.X, p.Y, 0.3, br);
            }
        }

        public bool IsCorrect() {
            for(int i = 0; i < coords?.Count; i++)
                for(int j = i + 1; j < coords.Count; j++)
                    if(AreCrossing(coords[i], coords[(i + 1) % coords.Count], coords[j], coords[(j + 1) % coords.Count], colideSegments: Math.Abs(i - j) > 1 && !(i == 0 && j == coords.Count - 1)))
                        return false;
            if (AreAreasIntersecting())
                return false;
            return true; 
        }

        public static bool AreCrossing(AreaCoords x1, AreaCoords y1, AreaCoords x2, AreaCoords y2, bool colideSegments = true) {
            double CrossProduct(AreaCoords p1, AreaCoords p2, AreaCoords p3) {
                return (p2.X - p1.X) * (p3.Y - p1.Y) - (p2.Y - p1.Y) * (p3.X - p1.X);
            }
            
            bool IsPointOnSegment(AreaCoords p, AreaCoords q, AreaCoords r) {
                if(!colideSegments && (p == q || p == r || q == r)) return false;
                return q.X <= Math.Max(p.X, r.X) && q.X >= Math.Min(p.X, r.X) &&
                       q.Y <= Math.Max(p.Y, r.Y) && q.Y >= Math.Min(p.Y, r.Y);
            }

            double cp1 = CrossProduct(x1, y1, x2);
            double cp2 = CrossProduct(x1, y1, y2);
            double cp3 = CrossProduct(x2, y2, x1);
            double cp4 = CrossProduct(x2, y2, y1);

            if((cp1 > 0 && cp2 < 0 || cp1 < 0 && cp2 > 0) && (cp3 > 0 && cp4 < 0 || cp3 < 0 && cp4 > 0))
                return true;

            if(cp1 == 0 && IsPointOnSegment(x1, x2, y1))
                return true;
            if(cp2 == 0 && IsPointOnSegment(x1, y2, y1))
                return true;
            if(cp3 == 0 && IsPointOnSegment(x2, x1, y2))
                return true;
            if(cp4 == 0 && IsPointOnSegment(x2, y1, y2))
                return true;
            return false;
        }

        public static bool AreAreasIntersecting(ObservableCollection<AreaCoords> area1, ObservableCollection<AreaCoords> area2) {
            if(area1 != null && area2 != null) {
                for(int i = 0; i < area1.Count; i++) {
                    for(int j = 0; j < area2.Count; j++) {
                        int nextI = (i + 1) % area1.Count;
                        int nextJ = (j + 1) % area2.Count;

                        if(AreCrossing(area1[i], area1[nextI], area2[j], area2[nextJ]))
                            return true;
                    }
                }
            }
            return false;
        }

        public bool AreAreasIntersecting() {
            foreach(var area1 in project?.Areas ?? new()) {
                foreach(var area2 in project?.Areas ?? new()) {
                    if(area1.Coords != null && area2.Coords != null && area1 != area2) {
                        for(int i = 0; i < area1.Coords.Count; i++) {
                            for(int j = 0; j < area2.Coords.Count; j++) {
                                int nextI = (i + 1) % area1.Coords.Count;
                                int nextJ = (j + 1) % area2.Coords.Count;

                                if(AreCrossing(area1.Coords[i], area1.Coords[nextI], area2.Coords[j], area2.Coords[nextJ])) return true;
                            }
                        }
                    }
                }
            }
            return false;
        }
    }
}
