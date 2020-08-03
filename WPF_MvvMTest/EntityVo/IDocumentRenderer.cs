using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace WPF_MvvMTest.EntityVo
{

    //定义接口
    public interface IDocumentRenderer
    {
        void Render(FlowDocument doc, Object data);
    }
}
