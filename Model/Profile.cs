using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using System.Windows.Media;

namespace AgeenkovApp.Model {
    public class Profile : PropChange {
        private int id;
        public int Id {
            get => id;
            set { id = value; OnPropertyChanged(nameof(Id)); }
        }

        private Area area;
        public Area Area {
            get => area;
            set { area = value; OnPropertyChanged(nameof(Area)); }
        }

        private ObservableCollection<Picket> pickets;
        public ObservableCollection<Picket> Pickets {
            get => pickets;
            set { pickets = value; OnPropertyChanged(nameof(Pickets)); }
        }

        private ObservableCollection<ProfileCoords> coords;
        public ObservableCollection<ProfileCoords> Coords {
            get => coords;
            set { coords = value; OnPropertyChanged(nameof(Coords)); }
        }

        public void Draw(DrawModel dm, Brush br) {
            if(coords is null) return;
            dm.DrawPoly(coords.Select(p => p.AsPoint).ToArray(), br, 0.08, false);
            foreach(var p in coords) {
                dm.DrawText($"{p.X};{p.Y}", p.X, p.Y, br, 0.5);
                dm.DrawCircle(p.X, p.Y, 0.1, br);
            }
        }

        public bool IsCorrect() {
            bool IsPointInsideArea(ProfileCoords point) {
                bool isInside = false;
                for(int i = 0, j = Area.Coords.Count - 1; i < Area.Coords.Count; j = i++) {
                    if((Area.Coords[i].Y > point.Y) != (Area.Coords[j].Y > point.Y) &&
                        (point.X < (Area.Coords[j].X - Area.Coords[i].X) * (point.Y - Area.Coords[i].Y) / (Area.Coords[j].Y - Area.Coords[i].Y) + Area.Coords[i].X)) {
                        isInside = !isInside;
                    }
                }
                return isInside;
            }

            for(int i = 0; i < coords?.Count - 1; i++)
                for(int j = 0; j < Area.Coords.Count; j++)
                    if(AreCrossing(coords[i].AsPoint, coords[i + 1].AsPoint, Area.Coords[j].AsPoint, Area.Coords[(j + 1) % Area.Coords.Count].AsPoint))
                        return false;
            foreach(var pr in Area?.Profiles ?? new())
                for(int i = 0; i < pr.Coords?.Count - 1; i++)
                    for(int j = 0; j < coords?.Count - 1; j++)
                        if(AreCrossing(pr.Coords[i].AsPoint, pr.Coords[i + 1].AsPoint, coords[j].AsPoint, coords[j + 1].AsPoint, colideSegments: pr == this ? Math.Abs(i - j) > 1 : true))
                            return false;

            if(coords != null) {
                foreach(var coord in coords) {
                    if(!IsPointInsideArea(coord))
                        return false;
                }
                if(coords.Any(p => coords.Count(p2 => p2.X == p.X && p2.Y == p.Y) > 1))
                    return false;
            }

            return true;
        }

        public static bool AreCrossing(Point x1, Point y1, Point x2, Point y2, bool colideSegments = true) {
            double CrossProduct(Point p1, Point p2, Point p3) {
                return (p2.X - p1.X) * (p3.Y - p1.Y) - (p2.Y - p1.Y) * (p3.X - p1.X);
            }

            bool IsPointOnSegment(Point p, Point q, Point r) {
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
    }
}
