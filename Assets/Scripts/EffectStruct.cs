public class EffectStruct
{
        public int x {get; private set;}
        public int y {get; private set;}
        public int direction {get; private set;}
        public int type {get; private set;}

        public EffectStruct(int _x,int _y, int _dir, int _type){
            x = _x;
            y = _y;
            direction = _dir;
            type = _type;

        }
}
