using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace RedStream
{
    public class RsGyroscope : RsActor
    {
        public enum GyroMode
        {
            OuterMaster,
            InnerMaster = 2,
            MaxMode
        };
        public enum RingType
        {
            InnerRing,
            MiddleRing,
            OuterRing,
            MaxRings
        };
        public int MaxTowersPerGyro=0, TowersPerGyroStep;
        public float GyroRotationSpeed, GyroAlignmentSpeed;
        public float GyroScale = 1.4f;
        public RsGyroscope(RsActor.Desc desc, RsGyroscopeData gyroData)
            : base(desc)
        {
            TowersPerGyroStep = gyroData.MaxTowersPerGyro;
            GyroRotationSpeed = gyroData.GyroRotationSpeed;
            GyroAlignmentSpeed = gyroData.GyroAlignmentSpeed;
            gyroDesc = desc;
            /*desc.MaterialDesc.Color = new Vector4(1, .4f, .7f, 1);
            desc.Name = "Outer";
            Rings[(int)RingType.OuterRing] = new RsGyro(desc, this);
            MaxTowersPerGyro -= MaxTowersPerGyro/4;
            desc.MaterialDesc.Color = new Vector4(.4f, 1, .7f, 1);
            desc.Scale *= 0.8f;
            desc.Name = "Middle";
            Rings[(int)RingType.MiddleRing] = new RsGyro(desc, this);
            MaxTowersPerGyro -= MaxTowersPerGyro / 3;
            desc.MaterialDesc.Color = new Vector4(.4f, .7f, 1, 1);
            desc.Scale *= 0.8f;
            desc.Name = "Inner";
            Rings[(int)RingType.InnerRing] = new RsGyro(desc, this);
            SelectedRing = (int)RingType.InnerRing;*/
            AddRing();
            Visible = false;
            Mode = GyroMode.InnerMaster;
            GyroRotations = new SmoothVectorComponent(Vector3.Zero, Vector3.Zero, 1, 360);
            GyroAlignments = new SmoothVectorComponent(Vector3.Zero, Vector3.Zero, 3, 360);
            //GyroRotations = new Vector3();
            //GyroAlignments = new Vector3();
            Colors[0] = new Vector3(0, 0, 1);
            Colors[1] = new Vector3(0, 1, 0);
            Colors[2] = new Vector3(1, 0, 0);
        }
        private RsActor.Desc gyroDesc;
        public int SelectedRing { get; private set; }
        public RsGyro[] Rings = new RsGyro[(int)RingType.MaxRings];
        private Vector3[] Colors = new Vector3[(int)RingType.MaxRings];
        private SmoothVectorComponent GyroRotations, GyroAlignments;
        //private Vector3 GyroRotations, GyroAlignments;
        private GyroMode Mode, DesiredMode;
        public int iLastRing = 0;
        public void AddRing()
        {
            if(iLastRing>=Rings.Length)
                return;
            MaxTowersPerGyro += TowersPerGyroStep;
            gyroDesc.MaterialDesc.Color = new Vector4(Colors[iLastRing], 1);
            gyroDesc.Name = ((RingType)iLastRing).ToString();
            Rings[iLastRing] = new RsGyro(gyroDesc, this);
            gyroDesc.Scale *= GyroScale;
            iLastRing++;
        }
        public void SelectNextRing()
        {
            SelectedRing = RsUtil.Mod(SelectedRing + 1, iLastRing); 
        }
        public void SelectPreviousRing()
        {
            SelectedRing = RsUtil.Mod(SelectedRing - 1, iLastRing);
        }
        public void AlignRing(float amount)
        {
            AlignRing((RingType)SelectedRing, amount);
        }
        public void RotateRing(float amount)
        {
            RotateRing((RingType)SelectedRing, amount);
        }
        public string GetSelectedRingName()
        {
            return GetRingName(Rings[SelectedRing]);
        }
        public string GetRingName(RsActor ring)
        {
            if (ring != null)
                return ring.Name;
            else return "None";
        }
        public string GetModeName()
        {
            return GetRingName(Rings[(int)Mode]);
        }
        public void ToggleMode()
        {
            if (Mode == GyroMode.InnerMaster)
                DesiredMode = GyroMode.OuterMaster;
            else if (Mode == GyroMode.OuterMaster)
                DesiredMode = GyroMode.InnerMaster;
            Reset();
        }
        public void Reset()
        {
            GyroAlignments.V = Vector3.Zero;
            GyroRotations.V = Vector3.Zero;
        }
        private void AlignRing(RingType selectedRing, float amount)
        {
            if (Mode != DesiredMode || !Rings[(int)selectedRing].Active) 
                return;
            Vector3 alignments = GyroAlignments.V;
            if (selectedRing == RingType.OuterRing)
            {
                alignments.X += amount;
            }
            else if (selectedRing == RingType.MiddleRing)
            {
                alignments.Y += amount;
            }
            else if (selectedRing == RingType.InnerRing)
            {
                alignments.Z += amount;
            }
            else return;
            GyroAlignments.V = alignments;
        }
        private void RotateRing(RingType selectedRing, float amount)
        {
            if (Mode != DesiredMode || !Rings[(int)selectedRing].Active)
                return;
            Vector3 rotations = GyroRotations.V;
            if (selectedRing == RingType.OuterRing)
            {
                rotations.X += amount;
            }
            else if (selectedRing == RingType.MiddleRing)
            {
                rotations.Y += amount;
            }
            else if (selectedRing == RingType.InnerRing)
            {
                rotations.Z += amount;
            }
            else return;
            GyroRotations.V = rotations;
        }
        private void ApplyRotations()
        {
            Quaternion OuterQuat, MiddleQuat, InnerQuat;
            //align the rings
            Vector3 alignment = GyroAlignments.V, rotation = GyroRotations.V;
            OuterQuat = Quaternion.CreateFromAxisAngle(new Vector3(1, 0, 0), MathHelper.ToRadians(alignment.X));
            MiddleQuat = Quaternion.CreateFromAxisAngle(new Vector3(0, 0, 1), MathHelper.ToRadians(alignment.Y));
            InnerQuat = Quaternion.CreateFromAxisAngle(new Vector3(1, 0, 0), MathHelper.ToRadians(alignment.Z));
            if (Mode == GyroMode.OuterMaster)
            {
                if (iLastRing > (int)RingType.OuterRing)
                    Rings[(int)RingType.OuterRing].Orientation = OuterQuat;
                if (iLastRing > (int)RingType.MiddleRing)
                    Rings[(int)RingType.MiddleRing].Orientation = OuterQuat * MiddleQuat;
                if (iLastRing > (int)RingType.InnerRing)
                    Rings[(int)RingType.InnerRing].Orientation = OuterQuat * MiddleQuat * InnerQuat;
            }
            else if (Mode == GyroMode.InnerMaster)
            {
                if (iLastRing > (int)RingType.InnerRing)
                    Rings[(int)RingType.InnerRing].Orientation = InnerQuat;
                if (iLastRing > (int)RingType.MiddleRing)
                    Rings[(int)RingType.MiddleRing].Orientation = InnerQuat * MiddleQuat;
                if (iLastRing > (int)RingType.OuterRing)
                    Rings[(int)RingType.OuterRing].Orientation = InnerQuat * MiddleQuat * OuterQuat;
            }
            //rotate the rings
            OuterQuat = Quaternion.CreateFromAxisAngle(new Vector3(0, 1, 0), MathHelper.ToRadians(rotation.X));
            MiddleQuat = Quaternion.CreateFromAxisAngle(new Vector3(0, 1, 0), MathHelper.ToRadians(rotation.Y));
            InnerQuat = Quaternion.CreateFromAxisAngle(new Vector3(0, 1, 0), MathHelper.ToRadians(rotation.Z));
            if (iLastRing > (int)RingType.InnerRing)
                Rings[(int)RingType.InnerRing].Orientation *= InnerQuat;
            if (iLastRing > (int)RingType.MiddleRing)
                Rings[(int)RingType.MiddleRing].Orientation *= MiddleQuat;
            if (iLastRing > (int)RingType.OuterRing)
                Rings[(int)RingType.OuterRing].Orientation *= OuterQuat;
        }
        private void UpdateRotations(float seconds)
        {

        }
        public override void Update(GameTime gameTime)
        {
            if (GyroAlignments.V == Vector3.Zero)
                Mode = DesiredMode;
            base.Update(gameTime);
            ApplyRotations();
            for (int i = 0; i < iLastRing; i++)
                if (Rings[i].Active)
                    Rings[i].Material.Color = new Vector4(Colors[i], 1);
                else Rings[i].Material.Color = new Vector4(0, 0, 0, 1);
        }
        public override void Die()
        {
            GyroAlignments.Delete();
            GyroRotations.Delete();
            base.Die();
        }
    }
}
