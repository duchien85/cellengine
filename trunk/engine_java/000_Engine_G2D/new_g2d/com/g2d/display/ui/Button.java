package com.g2d.display.ui;
import com.g2d.Color;
import com.g2d.Graphics2D;
import com.g2d.util.Drawing;

/**
 * 用于显示基本文字的按钮
 * @author WAZA
 */
public class Button extends BaseButton 
{
	/**文字颜色*/
	public Color 				unfocusTextColor	= new Color(0xffffFF00);
	
	/**文字颜色(获得鼠标后)*/
	public Color 				focusTextColor		= new Color(0xffffffff);
	
	/**text*/
	public String 				text 				= getClass().getSimpleName();
	
	/**text_anchor*/
	public int 					text_anchor			= Drawing.TEXT_ANCHOR_HCENTER | Drawing.TEXT_ANCHOR_VCENTER ;
	public int 					text_offset_x;
	public int 					text_offset_y;

	/**文字是否抗锯齿*/
	public boolean	enable_antialiasing	 = false;
	
	
	public Button(String text, int width, int height) {
		super(width, height);
		this.text = text;
	}
	
	public Button(String text) {
		super() ;
		this.text = text;
	}
	
	public Button() {
		super();
	}
	
	public void setText(String text) {
		this.text = text;
	}
	
	@Override
	public void render(Graphics2D g) {
		super.render(g);

		if (enable_antialiasing) {
			boolean flag = g.setFontAntialiasing(enable_antialiasing);
			render_text(g);
			g.setFontAntialiasing(flag);
		} else {
			render_text(g);
		}
		
	}
	
	protected void render_text(Graphics2D g)
	{
		if (getRoot()!=null && isCatchedMouse()) {
			g.setColor(focusTextColor);
		}else{
			g.setColor(unfocusTextColor);
		}
		if (isOnDragged()) {
			Drawing.drawStringBorder(g, text, text_offset_x, text_offset_y+1, getWidth(), getHeight(), text_anchor);
		}else{
			Drawing.drawStringBorder(g, text, text_offset_x, text_offset_y+0, getWidth(), getHeight(), text_anchor);
		}
	}
}