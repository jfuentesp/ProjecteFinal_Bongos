using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace m17
{
    public interface IState
    {
        public void Init();
        public void Exit();
    }
}
