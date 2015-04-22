using System;
using Android.Runtime;
using Android.Views;
using Android.Graphics;
namespace Mongoose.Droid
{
	public class LocationViewDroid : View, ILocationChangeListener
	{
		#region Paint
		private Paint _meshPaint;
		private Paint _textPaint;
		private Paint _youFillPaint;
		private Paint _poiFillPaint;
		private Paint _borderPaint;
		#endregion

		#region Drawn Data
		private string[] _drawnBuildingNames;
		private Canvas _canvas;
		#endregion

		#region Zoom Data
		private double _scale;
		private double _zoom;
		private double _originalPinchDistance;
		private double _currentPinchDistance;
		#endregion

		#region Positioning Data
		private Coordinates _youPos;
		private Coordinates _poiPos;
		private Coordinates _centerOffset;

		private double _moveScreenSpeed;

		private float _pointRadius;
		/// <summary>
		/// Gets the top left.
		/// </summary>
		/// <returns>The top left.</returns>
		private Coordinates GetTopLeft()
		{
			return new Coordinates(40.0, 0, 0); 
		}
		/// <summary>
		/// Gets the bottom right.
		/// </summary>
		/// <returns>The bottom right.</returns>
		private Coordinates GetBottomRight()
		{
			return new Coordinates(_canvas.Height, _canvas.Width, 0);
		}
		/// <summary>
		/// Gets the center.
		/// </summary>
		/// <returns>The center.</returns>
		private Coordinates GetCenter()
		{
			return _youPos + _centerOffset;
		}
		/// <summary>
		/// Gets the screen box.
		/// </summary>
		/// <returns>The screen box.</returns>
		private LocationBoundingBox GetScreenBox()
		{
			LocationBoundingBox result = new LocationBoundingBox(ScreenToWorldCentered(GetTopLeft()), ScreenToWorldCentered(GetBottomRight()));
			return result;
		}
		/// <summary>
		/// Gets the zoomed scale.
		/// </summary>
		/// <returns>The zoomed scale.</returns>
		private double GetZoomedScale()
		{
			return _scale * (_zoom * GetPinchZoom());
		}
		/// <summary>
		/// Gets the pinch zoom.
		/// </summary>
		/// <returns>The pinch zoom.</returns>
		private double GetPinchZoom()
		{
			return (_currentPinchDistance / _originalPinchDistance);
		}
		/// <summary>
		/// Resets the pinch.
		/// </summary>
		private void ResetPinch()
		{
			_currentPinchDistance = -1.0;
			_originalPinchDistance = -1.0;
		}
		/// <summary>
		/// Determines whether this instance is pinching.
		/// </summary>
		/// <returns><c>true</c> if this instance is pinching; otherwise, <c>false</c>.</returns>
		private bool IsPinching()
		{
			return _currentPinchDistance > 0 || _originalPinchDistance > 0;
		}
		#endregion
		/// <summary>
		/// Initializes a new instance of the <see cref="Mongoose.Droid.LocationViewDroid"/> class.
		/// </summary>
		/// <param name="context">Context.</param>
		public LocationViewDroid(Android.Content.Context context)
			: base(context)
		{
			_meshPaint = new Paint();
			_meshPaint.Color = Color.CornflowerBlue;
			_meshPaint.StrokeWidth = 4.0f;
			_meshPaint.SetStyle(Paint.Style.Stroke);

			_textPaint = new Paint();
			_textPaint.Color = Color.Black;
			_textPaint.TextSize = 30;

			_youFillPaint = new Paint();
			_youFillPaint.Color = Color.CornflowerBlue;
			_youFillPaint.SetStyle(Paint.Style.Fill);

			_poiFillPaint = new Paint();
			_poiFillPaint.Color = Color.Maroon;
			_poiFillPaint.SetStyle(Paint.Style.Fill);

			_borderPaint = new Paint();
			_borderPaint.Color = Color.Black;
			_borderPaint.StrokeWidth = 5.0f;
			_borderPaint.SetStyle(Paint.Style.Stroke);
		
			_drawnBuildingNames = new string[] 
			{
				"Home",
				"H",
				"EV",
				"LB",
				"MB"
			};
			_scale = 200000.0;
			_pointRadius = 20.0f;
			_moveScreenSpeed = 1.5f;
			_zoom = 1.0f;
			_youPos = Globals.LastLocation.Coords;
			_poiPos = null;
			_centerOffset = new Coordinates(0, 0, 0);
			ResetPinch();
		}
		/// <param name="canvas">the canvas on which the background will be drawn</param>
		/// <summary>
		/// Implement this to do your drawing.
		/// </summary>
		protected override void OnDraw(Canvas canvas)
		{
			base.OnDraw(canvas);
			_canvas = canvas;
			DrawBuildings(_drawnBuildingNames);

			DrawPoint(_youPos, _pointRadius, _borderPaint, _youFillPaint);
			DrawText("YOU " + _youPos, GetTopLeft());
			if(_poiPos != null)
			{
				DrawPoint(_poiPos, _pointRadius, _borderPaint, _poiFillPaint);
				DrawText("POI " + _poiPos, GetTopLeft() + new Coordinates(_textPaint.TextSize, 0, 0));
			}
			DrawText("Zoom: " + GetZoomedScale(), GetTopLeft() + new Coordinates(_textPaint.TextSize * 2, 0, 0));
		}
		/// <param name="e">The motion event.</param>
		/// <summary>
		/// Implement this method to handle touch screen motion events.
		/// </summary>
		/// <returns>To be added.</returns>
		public override bool OnTouchEvent(MotionEvent e)
		{
			MotionEventActions action = e.ActionMasked;
			switch(action)
			{
				case(MotionEventActions.Move):
					ProcessMove(e);
					break;
				case(MotionEventActions.Up):
					ProcessUp(e);
					break;
				default:
					break;
			}
			return true;
		}
		private void ProcessMove(MotionEvent e)
		{
			if(e.PointerCount == 2
				&& e.HistorySize > 0)
			{
				Coordinates distance = new Coordinates(e.GetY(0) - e.GetY(1), e.GetX(0) - e.GetX(1), 0);
				if(!IsPinching())
				{
					_originalPinchDistance =  distance.Length;
					_currentPinchDistance = distance.Length;
				}
				else
				{
					_currentPinchDistance = distance.Length;
				}
				Invalidate();
			}
			else if(e.PointerCount == 1 && e.HistorySize > 0)
			{
				Coordinates moveDistance = new Coordinates(e.GetY() - e.GetHistoricalY(e.HistorySize - 1),
					e.GetX() - e.GetHistoricalX(e.HistorySize - 1), 0);
				_centerOffset -= ScreenToWorld(moveDistance) * _moveScreenSpeed;
				Invalidate();
			}
		}
		private void ProcessUp(MotionEvent e)
		{
			if(IsPinching())
			{
				_zoom *= GetPinchZoom();
				ResetPinch();
			}
			else if(e.HistorySize == 0)
			{
				_poiPos = ScreenToWorldCentered(new Coordinates(e.GetY(), e.GetX(), 0));
				Invalidate();
			}
		}
		/// <summary>
		/// Draws the point.
		/// </summary>
		/// <param name="pos">Position.</param>
		/// <param name="radius">Radius.</param>
		/// <param name="strokePaint">Stroke paint.</param>
		/// <param name="fillPaint">Fill paint.</param>
		private void DrawPoint(Coordinates pos, float radius, Paint strokePaint, Paint fillPaint)
		{
			Coordinates offset = new Coordinates(radius, radius, 0);
			Coordinates screenPos = WorldToScreenCentered(pos);
			Coordinates clampedScreenPos = Coordinates.Clamp(screenPos, GetTopLeft() + offset, GetBottomRight() - offset);
			DrawCircle(clampedScreenPos, radius, fillPaint);
			DrawCircle(clampedScreenPos, radius, strokePaint);
		}
		/// <summary>
		/// Draws the circle.
		/// </summary>
		/// <param name="pos">Position.</param>
		/// <param name="radius">Radius.</param>
		/// <param name="paint">Paint.</param>
		private void DrawCircle(Coordinates pos, float radius, Paint paint)
		{
			if(paint != null)
			{
				_canvas.DrawCircle((float)pos.longitude, (float)pos.latitude, radius, paint);
			}
		}
		/// <summary>
		/// Draws the text.
		/// </summary>
		/// <param name="text">Text.</param>
		/// <param name="pos">Position.</param>
		private void DrawText(string text, Coordinates pos)
		{
			_canvas.DrawText(text, (float)pos.longitude, (float)pos.latitude, _textPaint);
		}
		/// <summary>
		/// Draws the buildings.
		/// </summary>
		/// <param name="buildingNames">Building names.</param>
		private void DrawBuildings(string[] buildingNames)
		{
			foreach(string name in buildingNames)
			{
				DrawBuilding(name);
			}
		}
		/// <summary>
		/// Draws the building.
		/// </summary>
		/// <param name="buildingName">Building name.</param>
		private void DrawBuilding(string buildingName)
		{
			LocationMesh buildingMesh = Globals.buildingAddressCollection.GetMesh(buildingName);
			if(buildingMesh != null)
			{
				DrawMesh(buildingMesh);
				DrawText(buildingName, WorldToScreenCentered(buildingMesh.GetCenter()));
			}
		}
		/// <summary>
		/// Draws the mesh.
		/// </summary>
		/// <param name="mesh">Mesh.</param>
		private void DrawMesh(LocationMesh mesh)
		{
			if(IsOnScreen(mesh))
			{
				LocationTriangle[] triangles = mesh.GetTriangles();
				foreach(LocationTriangle triangle in triangles)
				{
					DrawTriangle(triangle);
				}
			}
			else
			{
				DrawPoint(mesh.GetCenter(), _pointRadius, _meshPaint, null);
			}
		}
		/// <summary>
		/// Draws the triangle.
		/// </summary>
		/// <param name="triangle">Triangle.</param>
		private void DrawTriangle(LocationTriangle triangle)
		{
			Coordinates[] points = triangle.GetPoints();
			for(int i = 0; i < 3; i++)
			{
				DrawLine(points[i], points[(i + 1) % 3]);
			}
		}
		/// <summary>
		/// Draws the line.
		/// </summary>
		/// <param name="p1">Point1</param>
		/// <param name="p2">Ppoint2</param>
		private void DrawLine(Coordinates p1, Coordinates p2)
		{
			Coordinates screenP1 = WorldToScreenCentered(p1);
			Coordinates screenP2 = WorldToScreenCentered(p2);
			_canvas.DrawLine((float)screenP1.longitude, (float)(screenP1.latitude), 
				(float)screenP2.longitude, (float)(screenP2.latitude), 
				_meshPaint);
		}
		/// <summary>
		/// Worlds to screen centered.
		/// </summary>
		/// <returns>The to screen centered.</returns>
		/// <param name="p">P coordinates</param>
		private Coordinates WorldToScreenCentered(Coordinates p)
		{
			Coordinates result = p - GetCenter();
			result *= GetZoomedScale();
			result += GetScreenCenter();
			return result;
		}
		/// <summary>
		/// Invert the specified p.
		/// </summary>
		/// <param name="p">P coordinates</param>
		private Coordinates Invert(Coordinates p)
		{
			Coordinates result = new Coordinates(0, p.longitude, 0);
			result.latitude = GetScreenCenter().latitude - p.latitude;
			return p;
		}
		/// <summary>
		/// Screens to world centered.
		/// </summary>
		/// <returns>The to world centered.</returns>
		/// <param name="p">P coordinates</param>
		private Coordinates ScreenToWorldCentered(Coordinates p)
		{
			Coordinates result = p - GetScreenCenter();
			result /= GetZoomedScale();
			result += GetCenter();
			return result;
		}
		/// <summary>
		/// Worlds to screen.
		/// </summary>
		/// <returns>The to screen.</returns>
		/// <param name="p">P coordinates</param>
		private Coordinates WorldToScreen(Coordinates p)
		{
			Coordinates result = p * GetZoomedScale();
			return result;
		}
		/// <summary>
		/// Screens to world.
		/// </summary>
		/// <returns>The to world.</returns>
		/// <param name="p">P coordinates</param>
		private Coordinates ScreenToWorld(Coordinates p)
		{
			Coordinates result = p / GetZoomedScale();
			return result;
		}
		/// <summary>
		/// Gets the screen center.
		/// </summary>
		/// <returns>The screen center.</returns>
		private Coordinates GetScreenCenter()
		{
			Coordinates result = new Coordinates(Height, Width, 0) / 2;
			return result;
		}
		/// <summary>
		/// Determines whether this instance is on screen the specified mesh.
		/// </summary>
		/// <returns><c>true</c> if this instance is on screen the specified mesh; otherwise, <c>false</c>.</returns>
		/// <param name="mesh">Mesh for the triangle</param>
		private bool IsOnScreen(LocationMesh mesh)
		{
			bool result = mesh.GetBoundingBox().Intersects(GetScreenBox());
			return result;
		}
		#region ILocationChangeListener implementation
		/// <summary>
		/// Raises the location change event.
		/// </summary>
		/// <param name="newLocation">New location.</param>
		public void OnLocationChange(ILocation newLocation)
		{
			_youPos = newLocation.Coords;
			//redraw the whole view
			Invalidate();
		}
		#endregion
	}
}