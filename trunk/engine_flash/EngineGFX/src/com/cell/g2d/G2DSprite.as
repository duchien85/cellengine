package com.cell.g2d
{
	import com.cell.gameedit.ImageObserver;
	import com.cell.gameedit.ResourceLoader;
	import com.cell.gameedit.object.ImagesSet;
	import com.cell.gfx.game.CGameScene;
	import com.cell.gfx.game.CGraphicsDisplay;
	import com.cell.gfx.game.CSprite;
	import com.cell.gfx.game.IGraphics;
	
	import flash.display.BitmapData;
	import flash.display.Sprite;
	import flash.geom.Rectangle;

	public class G2DSprite extends Sprite implements ImageObserver
	{
		private var spr		: CSprite;
		private var cg		: IGraphics;
		
		public function G2DSprite(spr:CSprite)
		{
			this.spr = spr;
			this.cg = new CGraphicsDisplay(graphics);
			repaint();
		}
		
		public function get src() : CSprite
		{
			return spr;
		}
		
		public function getCSprite() : CSprite
		{
			return spr;
		}
		
		public function repaint() : void
		{
			graphics.clear();
			spr.render(cg, 0, 0, spr.getCurrentAnimate(), spr.getCurrentFrame());
			graphics.moveTo( 0, 0);
			{
				graphics.lineStyle(2, 0xffff0000, 1);
				graphics.lineTo(-8, 0); graphics.moveTo( 0, 0);
				graphics.lineTo( 8, 0); graphics.moveTo( 0, 0);
				graphics.lineTo( 0,-8); graphics.moveTo( 0, 0);
				graphics.lineTo( 0, 8); graphics.moveTo( 0, 0);
				graphics.endFill();
			}
		}
		
		public function imagesLoaded(res:ResourceLoader, img:ImagesSet) : void
		{
			repaint();
		}
		
	}
}