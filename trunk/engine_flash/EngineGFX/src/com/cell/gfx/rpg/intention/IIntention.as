package com.cell.gfx.rpg.intention
{
	import com.cell.gfx.rpg.G2DUnit;

	public interface IIntention
	{
		function onUpdate(unit:G2DUnit) : void ;
		
		function onStart(unit:G2DUnit) : void ;
		
		function onStop(unit:G2DUnit) : void ;
	}
}