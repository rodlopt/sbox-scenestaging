﻿using Sandbox;
using Sandbox.Diagnostics;
using System;

[Title( "Directional Light" )]
[Category( "Light" )]
[Icon( "light_mode", "red", "white" )]
public class DirectionalLightComponent : GameObjectComponent
{
	SceneSunLight _sceneObject;

	[Property] public Color LightColor { get; set; } = "#E9FAFF";
	[Property] public Color SkyColor { get; set; } = "#0F1315";

	[Property] public bool Shadows { get; set; } = true;

	public override void DrawGizmos()
	{
		using var scope = Gizmo.Scope( $"{GetHashCode()}", GameObject.Transform );

		var fwd = Vector3.Forward;

		Gizmo.Draw.Color = LightColor;

		for ( float f = 0; f<MathF.PI * 2; f+= 0.5f  )
		{
			var x = MathF.Sin( f );
			var y = MathF.Cos( f );

			var off = (x * Vector3.Left + y * Vector3.Up) * 5.0f;

			Gizmo.Draw.Line( off, off + fwd * 30 );
		}

	}

	public override void OnEnabled()
	{
		Assert.True( _sceneObject == null );
		Assert.NotNull( Scene );

		_sceneObject = new SceneSunLight( Scene.SceneWorld, GameObject.Transform.Rotation, Color.White );
		_sceneObject.Transform = GameObject.WorldTransform;
		_sceneObject.ShadowsEnabled = true;
	}

	public override void OnDisabled()
	{
		_sceneObject?.Delete();
		_sceneObject = null;
	}

	protected override void OnPreRender()
	{
		if ( !_sceneObject.IsValid() )
			return;

		_sceneObject.Transform = GameObject.WorldTransform;
		_sceneObject.ShadowsEnabled = Shadows;
		_sceneObject.LightColor = LightColor;
		_sceneObject.SkyColor = SkyColor;

		_sceneObject.ShadowCascadeCount = 3;
		_sceneObject.SetShadowCascadeDistance( 0, 200 );
		_sceneObject.SetShadowCascadeDistance( 1, 500 );
		_sceneObject.SetShadowCascadeDistance( 2, 5000 );
	}

}
