package com.cell.g2d
{
	import com.cell.gameedit.OutputLoader;
	import com.cell.gameedit.ResourceEvent;
	import com.cell.gameedit.ResourceLoader;
	import com.cell.gameedit.object.ImagesSet;
	import com.cell.gameedit.object.SpriteSet;
	import com.cell.gameedit.object.WorldSet;
	import com.cell.gameedit.object.worldset.SpriteObject;
	import com.cell.gfx.CellSprite;
	import com.cell.gfx.game.CCamera;
	import com.cell.gfx.game.CGraphicsDisplay;
	import com.cell.gfx.game.CMap;
	import com.cell.gfx.game.CSprite;
	import com.cell.gfx.game.IGraphics;
	import com.cell.gfx.game.IImageObserver;
	
	import flash.display.Sprite;
	import flash.geom.Rectangle;

	public class G2DScene extends Sprite
	{
		private var resource 	: ResourceLoader;
		private var world_data	: WorldSet;
		
		public function G2DScene(
			res:ResourceLoader,
			world:WorldSet,
			viewWidth:int, 
			viewHeight:int)
		{
//			graphics.beginFill(0x00ff00, 1);
//			graphics.drawRect(0, 0, world.Width, world.Height);
//			graphics.endFill();			
			this.resource = res;
			this.world_data = world;
			this.scrollRect = new Rectangle(0, 0, viewWidth, viewHeight);
//			this.opaqueBackground  = false;
//			// Force DisplayObject update dimensions
//			var bmpData:BitmapData = new BitmapData(1, 1);
//			bmpData.draw(mc);
			for each (var obj:SpriteObject in world.Sprs) {
				var unit : CellSprite = createUnit(obj);
				if (unit != null) {
					unit.x = obj.X;
					unit.y = obj.Y;
					unit.getCSprite().setCurrentAnimate(obj.Anim);
					unit.getCSprite().setCurrentFrame(obj.Frame);
					addChild(unit);
				}
			}
//			unit_pan.width = world.Width;
//			unit_pan.height = world.Height;
//			this.cacheAsBitmap = true;
		}
		
		public function getResource() : ResourceLoader
		{
			return resource;
		}
		
		public function getWorldSet() : WorldSet
		{
			return world_data;
		}
		
		protected function createUnit(obj:SpriteObject) : CellSprite 
		{
			var cspr : CSprite = resource.getSprite(obj.SprID);
			var ret : CellSprite = new CellSprite(cspr);
			return ret;
		}
		
		public function getCameraX() : int
		{
			return this.scrollRect.x;
		}
		
		public function getCameraY() : int
		{
			return this.scrollRect.y;
		}
		
		public function getCameraWidth()  : int
		{
			return this.scrollRect.width;
		}
		
		public function getCameraHeight() : int
		{
			return this.scrollRect.height;
		}
		
		public function setCameraSize(w:int, h:int) : void
		{
			var rect : Rectangle = scrollRect;
			
			if (rect.width != w || rect.height != h)	
			{
				if (world_data != null) 
				{
					if (w > world_data.Width) {
						w = world_data.Width;
					}
					if (h > world_data.Height) {
						h = world_data.Height;
					}
				}
				rect.width = w;
				rect.height = h;
				
				this.scrollRect = rect;
			}
		}
		
		public function locateCamera(x:int, y:int) : void 
		{
			var rect : Rectangle = scrollRect;
			
			if (rect.x != x || rect.y != y)		
			{
				if (x < 0) {
					x = 0;
				}
				if (x > world_data.Width - rect.width) {
					x = world_data.Width - rect.width;
				}
				if (y < 0) {
					y = 0;
				}
				if (y > world_data.Height - rect.height) {
					y = world_data.Height - rect.height;
				}
				rect.x = x ;
				rect.y = y ;
				
				this.scrollRect = rect;
			}
		}
		
		public function locateCameraCenter(x:int, y:int) : void 
		{
			locateCamera(x - scrollRect.width/2, y - scrollRect.height/2);
		}
		
		public function moveCamera(dx:int, dy:int) : void 
		{
			locateCamera(getCameraX() + dx, getCameraY() + dy);
		}
	}
	
	
}