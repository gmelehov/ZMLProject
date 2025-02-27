

Ext.require('Ext.ux.grid.FiltersFeature');
Ext.require('Ext.ux.grid.menu.ListMenu');
Ext.require('Ext.ux.grid.filter.StringFilter');
Ext.require('Ext.ux.grid.filter.ListFilter');
Ext.require('Ext.ux.grid.filter.NumericFilter');






/** РАСШИРЕНИЕ СТАНДАРТНОГО МЕНЕДЖЕРА ХРАНИЛИЩ ДАННЫХ */
function ExtendExtStoreManager()
{
  Ext.StoreManager.LINKS = [];
  Ext.StoreManager.LINKS_MAP = {};



  Ext.StoreManager.on('add', function (indx, stor, key)
  {
    if (stor && Array.isArray(stor.paramLinkers) && stor.paramLinkers.length && !Ext.StoreManager.LINKS_MAP[key])
    {
      Ext.StoreManager.LINKS.push({
        storeId: key,
        ObjectType: stor.ObjectType,
        linkers: [].concat(stor.paramLinkers)
      });
      Ext.StoreManager.LINKS_MAP[key] = stor.paramLinkers.length;
    };
  }, Ext.StoreManager);




  Ext.StoreManager.QueryLinks = function (query)
  {
    if (query)
    {
      var qry = Ext.Object.getKeys(query);
      return Ext.StoreManager.LINKS.filter(function (f)
      {
        return f.linkers.some(function (s)
        {
          return qry.every(function (ev)
          {
            return s[ev] == query[ev];
          });
        })
      })
    };
  };





  Ext.StoreManager.GetLinkedStores = function (query)
  {
    return Ext.StoreManager.QueryLinks(query).filter(function (f) { return f.ObjectType == 'source'; }).map(function (m)
    {
      return Ext.getStore(m.storeId)
    });
  };





  Ext.StoreManager.LoadLinkedStores = function (query)
  {
    Ext.StoreManager.GetLinkedStores(query).forEach(function (f)
    {
      f.load();
    });
  };




  Ext.StoreManager.SOURCE = function (id)
  {
    return Ext.StoreManager.items.filter(function (f)
    {
      return f.ObjectType == 'source' && (id ? f.storeId == id : true);
    });
  };





  Ext.StoreManager.ACTION = function (id)
  {
    return Ext.StoreManager.items.filter(function (f)
    {
      return f.ObjectType == 'action' && (id ? f.storeid == id : true);
    });
  };





  /**
   * Метод для загрузки всех хранилищ данных, имеющих свойство autoLoad = true.
   * Предназначен для автоматической инициализации хранилищ данных,
   * вызывается однократно при запуске приложения.
   */
  Ext.StoreManager.AutoloadSources = function ()
  {
    Ext.StoreManager.SOURCE().filter(function (stor) { return stor.autoLoad === true && stor.totalCount === 0; }).forEach(function (st)
    {
      st.load();
    });
  };




  Ext.StoreManager.NEWRECORD = function (linkers)
  {
    if (Array.isArray(linkers) && linkers.every(function (ev) { return ev.isParam; }))
    {
      linkers.forEach(function (f) { f.compute(); });
      var record = {};
      linkers.forEach(function (f)
      {
        record[f.as] = f.value;
      });
      record.phantom = true;
      return record;
    };
  };





  Ext.StoreManager.SAVENEWRECORD = function (id, linkers)
  {
    if (id && Ext.StoreManager.SOURCE(id) && Array.isArray(linkers) && linkers.every(function (ev) { return ev.isParam; }))
    {
      var rec = Ext.StoreManager.NEWRECORD(linkers);
      rec.phantom = true;
      //reс.setDirty();
      Ext.StoreManager.SOURCE(id)[0].AddAndSaveRecord(rec);
    };
  };



}

ExtendExtStoreManager();







Ext.define('GLOBAL', {

  extend: 'Ext.Component',

  singleton: true,









  CreateApp: function (appname)
  {
    GLOBAL[appname] = {

      Action: new ZML.ActionMgr(),
      Button: new ZML.ButtonMgr(),
      Chart: new ZML.ChartMgr(),
      Combo: new ZML.ComboMgr(),
      Dialog: new ZML.DialogMgr(),
      Editor: new ZML.EditorMgr(),
      Form: new ZML.FormMgr(),
      Grid: new ZML.GridMgr(),
      Input: new ZML.InputMgr(),
      List: new ZML.ListMgr(),
      Loader: new ZML.LoaderMgr(),
      Panel: new ZML.PanelMgr(),
      Tabpanel: new ZML.TabpanelMgr(),
      Tree: new ZML.TreeMgr(),
      Source: {},

    };
  }

});





Ext.define('ZML.AbstractMixedCollection', {

  extend: 'Ext.util.MixedCollection',


  listeners: {
    add: function (index, obj, key, eOpts)
    {
      this[key] = obj;
      this.OnItemAdded.bind(this);
    }
  },





  register: function ()
  {
    for (var i = 0, s; (s = arguments[i]); i++)
    {
      this.add(s);
    };
  },


  getKey: function (obj)
  {
    return obj.itemId;
  },


  regObject: function (name, config)
  {
    if (Ext.isObject(name))
    {
      config = name;
    }
    else
    {
      config.itemId = name;
    };
    return this.register(config);
  },





  OnItemAdded: function (index, obj, key, eOpts)
  {

  },


  EvaluateField: function ()
  {

  },

});








/** МЕНЕДЖЕР ИСПОЛНЯЕМЫХ ДЕЙСТВИЙ */
Ext.define('ZML.ActionMgr', {

  extend: 'ZML.AbstractMixedCollection',



  listeners: {
    add: function (index, obj, key, eOpts)
    {
      this[key] = obj.Handler.bind(obj);
      this.OnItemAdded.bind(this);
    }
  },

});





/** МЕНЕДЖЕР КНОПОК */
Ext.define('ZML.ButtonMgr', {

  extend: 'ZML.AbstractMixedCollection',


});




/** МЕНЕДЖЕР КОМБОБОКСОВ */
Ext.define('ZML.ComboMgr', {

  extend: 'ZML.AbstractMixedCollection',



  EvaluateField: function (itemId, field, record)
  {
    var found = this[itemId];
    var ret;
    if (record == null || record == undefined || isNaN(parseInt(record)))
    {
      record = -1;
    };
    if (found)
    {
      switch (field)
      {
        case 'display':
        case 'displayField': ret = found.getDisplayValue(); break;

        case 'descr':
        case 'descrField': ret = found.getDescriptionValue(); break;

        case 'value':
        case 'valueField': ret = found.getValue(); break;


        default:
          var temp = found.lastSelection.map(function (m) { return m.data[field] });
          ret = record > -1 ? temp[record] : temp;
          break;
      };
    };
    return ret;
  },

});




/** МЕНЕДЖЕР ДИАЛОГОВЫХ ОКОН */
Ext.define('ZML.DialogMgr', {

  extend: 'ZML.AbstractMixedCollection',


});




/** МЕНЕДЖЕР ТАБЛИЦ ДАННЫХ */
Ext.define('ZML.GridMgr', {

  extend: 'ZML.AbstractMixedCollection',




  EvaluateField: function (itemId, field, record)
  {
    var ret;
    var found = this[itemId] || null;
    record = +record;
    if (found)
    {
      var keytype = found.GetKeyType();
      var nullValue;
      switch (keytype)
      {
        case 'string': nullValue = null; break;


        default: nullValue = -1; break;
      };


      var selArray = found.getSelectionModel().getSelection();

      if (selArray.length > 1)
      {
        ret = found.GetSelectedRowField(field);
      }
      else
        if (record == null || record == undefined || record == 0)
        {

          var selected = found.getSelectionModel().getSelection()[0];
          ret = selected ? selected.get(field) : nullValue;
        }
        else
          if (!isNaN(parseInt(record)))
          {
            var foundRecord = found.getStore().getAt(record);
            ret = foundRecord ? foundRecord.get(field) : nullValue;
          };

      //if (!isNaN(parseInt(record)))
      //{
      //  switch (record)
      //  {
      //    case 0:
      //    case undefined:
      //    case null: ret = selArray[0] ? selArray[0].get(field) : found.getStore().getAt(0)?.get(field); break;

      //    default: ret = selArray[record] ? selArray[record].get(field) : found.getStore().getAt(record).get(field); break;
      //  };
      //}
      //else
      //{
      //  //ret = found.getStore().getAt(record).get(field);
      //};
    };
    return ret;
  },

});




/** МЕНЕДЖЕР ПАНЕЛЕЙ */
Ext.define('ZML.PanelMgr', {

  extend: 'ZML.AbstractMixedCollection',


});




/** МЕНЕДЖЕР ПОЛЕЙ ВВОДА ДАННЫХ */
Ext.define('ZML.InputMgr', {

  extend: 'ZML.AbstractMixedCollection',




  EvaluateField: function (itemId)
  {
    var found = this[itemId];
    return found ? found.getSubmitValue() : null;
  },

});




/** МЕНЕДЖЕР ПАНЕЛЕЙ СО ВКЛАДКАМИ */
Ext.define('ZML.TabpanelMgr', {

  extend: 'ZML.AbstractMixedCollection',



  EvaluateField: function (itemId, field)
  {

  },

});




/** МЕНЕДЖЕР ДЕРЕВЬЕВ ДАННЫХ */
Ext.define('ZML.TreeMgr', {

  extend: 'ZML.AbstractMixedCollection',



  EvaluateField: function (itemId, field, record)
  {

  },

});




/** МЕНЕДЖЕР РЕДАКТОРОВ КОНТЕНТА */
Ext.define('ZML.EditorMgr', {

  extend: 'ZML.AbstractMixedCollection',



  EvaluateField: function (itemId)
  {
    var found = this[itemId];
    return found ? found.getCodeValue() : null;
  },

});




/** МЕНЕДЖЕР ФОРМ ВВОДА ДАННЫХ */
Ext.define('ZML.FormMgr', {

  extend: 'ZML.AbstractMixedCollection',



  EvaluateField: function (itemId, field)
  {
    //field = '#' + field;
    //var found = this[itemId] ? this[itemId].query(field)[0] : null;
    var found = this[itemId];
    return found ? found.getSubmitValue() : null;
  },

});




/** МЕНЕДЖЕР СПИСКОВ ДАННЫХ */
Ext.define('ZML.ListMgr', {

  extend: 'ZML.AbstractMixedCollection',



  EvaluateField: function (itemId, field, record)
  {

  },

});




/** МЕНЕДЖЕР ГРАФИКОВ И ДИАГРАММ */
Ext.define('ZML.ChartMgr', {

  extend: 'ZML.AbstractMixedCollection',

});




/** МЕНЕДЖЕР ЗАГРУЗЧИКОВ */
Ext.define('ZML.LoaderMgr', {

  extend: 'ZML.AbstractMixedCollection',



  listeners: {
    add: function (index, obj, key, eOpts)
    {
      this[key] = obj;
      this.OnItemAdded.bind(this);
    }
  },



  EvaluateField: function (itemId, field)
  {
    var found = this[itemId];
    if (!!found)
    {
      switch (field)
      {
        case 'url': return found.GetUrl();
        case 'mode': return found.GetRenderMode();


        default: return null;
      };
    };
  },

});










GLOBAL.CreateApp('APP');















Ext.define('ZML.mixin.Loadable', {



  InitLoader: function ()
  {
    var me = this;
    if (me.__loader)
    {
      me.SetLoader(me.__loader);
      me.__loader = undefined;
    };
  },


  GetLoaderId: function ()
  {
    var me = this;
    if (me.getLoader())
    {
      return me.getLoader().itemId;
    };
  },


  SetLoader: function (loaderId)
  {
    var me = this;
    var loaderExists = !!GLOBAL[me.AppNAME].Loader[loaderId];
    if (loaderExists)
    {
      if (me.getLoader())
      {
        me.getLoader().UnlinkTarget();
      };

      me.loader = GLOBAL[me.AppNAME].Loader[loaderId];

      setTimeout(() => me.getLoader().load(), 0);
    };
  },


});




Ext.define('ZML.mixin.HandleClearIcon', {


  updateClearIcon: function ()
  {
    var me = this;
    var cls = 'saki-gms-hasvalue';
    var body = me.bodyEl ? me.bodyEl.down(me.inputElem) : null;
    var value = me.getValue ? me.getValue() : null;
    if (body)
    {
      body = body.up('td');
      me.clearIcon = me.bodyEl.dom.querySelector('div.saki-gms-clear');
      if (!me.clearIcon)
      {
        me.clearIcon = body.createChild({ tag: 'div', cls: 'saki-gms-clear' });
        me.clearIcon.on('click', Ext.bind(me.clearField, me, [me]));
        body.applyStyles({ position: 'relative' });
      };
      if (value && !me.readOnly && !me.disabled)
      {
        body.addCls(cls);
      }
      else
      {
        body.removeCls(cls);
      };
    };
  },


  clearField: function ()
  {
    var me = this;
    if (me && Ext.isFunction(me.setValue) && !me.readOnly && !me.disabled)
    {
      if (typeof me.ComposeEmptyDisplayTpl === 'function' && me.descrField && me.descrField != me.displayField)
      {
        me.displayTpl = Ext.create('Ext.XTemplate', me.ComposeEmptyDisplayTpl(me.displayField).outerHTML);
      };
      me.setValue('');
      me.clearIcon = null;
      me.updateClearIcon();
    };
  }
});




Ext.define('ZML.mixin.ProcessQuotes', {

  convertQuotes: false,


  CountQuotes: function (quote)
  {
    var me = this;
    var val = me.getValue();
    quote = quote || '"';
    var count = val.split('').filter(function (f)
    {
      return f === quote;
    }).length;
    return count;
  },


  ReplaceQuotes: function ()
  {
    var me = this;
    var val = me.getValue();
    var openQuotes = me.CountQuotes('«');
    var closeQuotes = me.CountQuotes('»');

    switch ((openQuotes + closeQuotes) % 2)
    {
      case 0:
        me.setValue(val.split('').join('').replace('"', '«').split('').reverse().join('').replace('"', '»').split('').reverse().join(''));
        break;

      case 1:
        if (openQuotes > closeQuotes)
        {
          me.setValue(val.split('').reverse().join('').replace('"', '»').split('').reverse().join(''));
        }
        else
        {
          me.setValue(val.split('').join('').replace('"', '«'));
        };
        break;

      default:
        break;
    };
  }
});




Ext.define('ZML.mixin.AfterEventCallback', {


  OnAfterEvent: function (callback, scope)
  {
    if (typeof callback === 'function')
    {
      Ext.Function.defer(callback, 0, scope, arguments);
    };
  },


  OnAfterPropertyChanged: function ()
  {

  }
});




Ext.define('ZML.mixin.FormFieldCommons', {


  OnFormatChanged: function (obj, oldvalue, newvalue)
  {
    if (obj)
    {
      obj.OnAfterPropertyChanged();
    };
  },


  OnValiditychange: function (obj, oldvalue, newvalue)
  {
    if (obj)
    {
      obj.OnAfterPropertyChanged();
      setTimeout(function ()
      {
        obj.OnAfterEvent(obj.ApiOnValiditychange, obj);
      }, 0);
    };
  },


  OnWriteablechange: function (obj, oldvalue, newvalue)
  {
    if (obj)
    {
      obj.OnAfterPropertyChanged();
    };
  },


  ApiOnValiditychange: null,

  ApiOnChange: null
});




Ext.define('ZML.plugin.ColorManager', {

  extend: 'Ext.AbstractPlugin',
  alias: 'plugin.ColorManager',


  mixins: {
    observable: 'Ext.util.Observable',
  },



  MENU: null,


  config: {

    /** Цвет текста заголовка для управляемого плагином элемента */
    titleColor: null,

    /** Цвет фона заголовка для управляемого плагином элемента */
    titleBackColor: null,

    /** Цвет фона содержимого для управляемого плагином элемента */
    bodyBackColor: null,


    showTool: false,

  },



  setTitleColor: function (value)
  {
    this.titleColor = value;
    this.UpdateTitleColor();
  },

  setTitleBackColor: function (value)
  {
    this.titleBackColor = value;
    this.UpdateTitleBackColor();
  },

  setBodyBackColor: function (value)
  {
    this.bodyBackColor = value;
    this.UpdateBodyBackColor();
  },






  init: function (comp)
  {
    var me = this;
    comp.COLOR_MAN = me;

    var conf = {
      titleColor: me.titleColor,
      titleBackColor: me.titleBackColor,
      bodyBackColor: me.bodyBackColor,
      showTool: me.showTool,
    };


    me.callParent(arguments);
    me.mixins.observable.constructor.call(me, conf);
    me.addEvents('colorsInitDone');

    me.on({
      colorsInitDone: me.OnColorsInitDone,
      scope: me
    });



    comp.on('afterrender', function (obj)
    {

      if (me.showTool === 'true')
      {
        if (obj.header && obj.title && !obj.tools.find(function (f) { return f.itemId == (obj.itemId + '-colorManTlbToggler') }))
        {
          obj.addTool({
            type: 'gear',
            itemId: obj.itemId + '-colorManTlbToggler',
            HOST: obj,
            callback: function (owner, tool, ev)
            {
              tool.HOST.COLOR_MAN.ShowMenu(ev);
            }
          });
        }
        else if (comp.tabBar && !comp.tabBar.tools.find(function (f) { return f.itemId == (comp.itemId + '-colorManTlbToggler') }))
        {
          comp.tabBar.add({ xtype: 'tbfill', flex: 1 });
          comp.tabBar.addTool({
            type: 'gear',
            itemId: comp.itemId + '-colorManTlbToggler',
            HOST: comp,
            callback: function (owner, tool, ev)
            {
              tool.HOST.COLOR_MAN.ShowMenu(ev);
            }
          });
        };
      };
     

      comp.COLOR_MAN.MENU = comp.COLOR_MAN.ComposeMenu();

      //comp.UpdateTitleBackColor();
      //comp.UpdateTitleColor();
      //comp.UpdateBodyBackColor();
    });


    me.InitColors();
  },




  OnColorsInitDone: function (obj)
  {
    obj.UpdateTitleBackColor();
    obj.UpdateTitleColor();
    obj.UpdateBodyBackColor();
  },








  UpdateTitleBackColor: function ()
  {
    var me = this;
    var check = !!me.cmp.header && !!me.cmp.header.el && !!me.cmp.header.el.dom && !!me.titleBackColor;
    if (!!check)
    {
      me.cmp.header.el.dom.style.setProperty('background-color', me.Colors[me.titleBackColor].color);
    }
    else if (!!me.cmp.tabBar)
    {
      me.cmp.tabBar.el.dom.style.setProperty('background-color', me.Colors[me.titleBackColor].color);
    };
  },


  UpdateTitleColor: function ()
  {
    var me = this;
    var check = !!me.cmp.header && typeof me.cmp.header.query === 'function' && !!me.titleColor;
    if (!!check && !!me.cmp.header.query('*[ariaRole=heading]')[0].textEl)
    {
      me.cmp.header.query('*[ariaRole=heading]')[0].textEl.el.dom.style.setProperty('color', me.Colors[me.titleColor].color);
    }
    else if (!!me.cmp.tabBar)
    {
      Ext.Array.from(me.cmp.tabBar.el.dom.querySelectorAll('.x-tab-inner')).forEach(function (f)
      {
        f.style.setProperty('color', me.Colors[me.titleColor].color);
      });
    };
  },


  UpdateBodyBackColor: function ()
  {
    var me = this;
    var check = typeof me.cmp.setBodyStyle === 'function' && !!me.bodyBackColor;
    if (!!check)
    {
      me.cmp.setBodyStyle('background-color', me.Colors[me.bodyBackColor].color);
    };
  },







  ComposeMenu: function ()
  {
    var me = this;

    var colormenuitems1 = Ext.Object.getKeys(me.Colors).map(function (m)
    {
      return {
        xtype: 'menucheckitem',
        text: m,
        checked: me.getTitleBackColor() == m,
        handler: function (it)
        {
          it.parentMenu.UncheckAll();
          it.parentMenu.parentMenu.HOST.setTitleBackColor(it.text);
          it.setChecked(true);
        }
      };
    });
    var colormenuitems2 = Ext.Object.getKeys(me.Colors).map(function (m)
    {
      return {
        xtype: 'menucheckitem',
        text: m,
        checked: me.getTitleColor() == m,
        handler: function (it)
        {
          it.parentMenu.UncheckAll();
          it.parentMenu.parentMenu.HOST.setTitleColor(it.text);
          it.setChecked(true);
        }
      };
    });
    var colormenuitems3 = Ext.Object.getKeys(me.Colors).map(function (m)
    {
      return {
        xtype: 'menucheckitem',
        text: m,
        checked: me.getBodyBackColor() == m,
        handler: function (it)
        {
          it.parentMenu.UncheckAll();
          it.parentMenu.parentMenu.HOST.setBodyBackColor(it.text);
          it.setChecked(true);
        }
      };
    });

    var menu = new Ext.menu.Menu({
      id: me.cmp.itemId + '-ColorManMenu',
      animCollapse: true,
      allowOtherMenus: true,
      defaults: { clickHideDelay: 200 },
      HOST: me,
      plain: true,
      ignoreParentClicks: true,
      menutarget: 'titleBodyColor',
      items: [
        {
          itemId: 'Item1',
          text: 'Цвет фона заголовка',
          clickHideDelay: 50,
          //plain: true,
          menu: {
            animCollapse: true,
            plain: true,
            defaults: { clickHideDelay: 200 },
            items: colormenuitems1,
            UncheckAll: function ()
            {
              this.query('menucheckitem[checked=true]').forEach(function (f) { f.setChecked(false); });
            }
          }
        },
        {
          itemId: 'Item2',
          text: 'Цвет текста заголовка',
          clickHideDelay: 50,
          //plain: true,
          menu: {
            animCollapse: true,
            plain: true,
            defaults: { clickHideDelay: 200 },
            items: colormenuitems2,
            UncheckAll: function ()
            {
              this.query('menucheckitem[checked=true]').forEach(function (f) { f.setChecked(false); });
            }
          }
        },
        {
          itemId: 'Item3',
          text: 'Цвет фона содержимого',
          clickHideDelay: 50,
          //plain: true,
          menu: {
            animCollapse: true,
            plain: true,
            defaults: { clickHideDelay: 200 },
            items: colormenuitems3,
            UncheckAll: function ()
            {
              this.query('menucheckitem[checked=true]').forEach(function (f) { f.setChecked(false); });
            }
          }
        }
      ]
    });

    return menu;
  },


  ShowMenu: function (ev)
  {
    var toolCont = this.cmp.header || this.cmp.tabBar;
    this.MENU.showAt(ev.getXY());
    this.MENU.alignTo(toolCont, 'tr-br', [-5, -5], false);
  },


  InitColors: function ()
  {
    var me = this;
    Ext.Object.getKeys(me.Colors).forEach(function (cc)
    {
      var c = me.Colors[cc];
      if (c.color === undefined && typeof c.Compute !== 'function')
      {
        var csscolor = Ext.create('ZML.CSSColor', c);
        csscolor.enable();
        me.Colors[cc] = csscolor;
      };
    });

    setTimeout(function () { me.fireEvent('colorsInitDone', me) }, 50);
  },


  Colors: {
    black: { hue: 0, sat: 0, light: 0, alpha: 1 },
    white: { hue: 0, sat: 0, light: 100, alpha: 1 },
    transparent: { hue: 0, sat: 0, light: 0, alpha: 0 },
    darkopaque: { hue: 0, sat: 0, light: 0, alpha: 0.1 },
    lightopaque: { hue: 0, sat: 0, light: 100, alpha: 0.1 },
    darkred: { hue: 0, sat: 90, light: 30, alpha: 1 },
    red: { hue: 0, sat: 52, light: 44, alpha: 1 },
    lightred: { hue: 0, sat: 42, light: 60, alpha: 1 },
    darkgrey: { hue: 0, sat: 0, light: 28, alpha: 1 },
    grey: { hue: 0, sat: 0, light: 50, alpha: 1 },
    cleargrey: { hue: 0, sat: 0, light: 82, alpha: 1 },
    lightgrey: { hue: 0, sat: 0, light: 88, alpha: 1 },
    slightgrey: { hue: 0, sat: 0, light: 92, alpha: 1 },
    palegrey: { hue: 0, sat: 0, light: 96, alpha: 1 },
    darkbrown: { hue: 38, sat: 96, light: 22, alpha: 1 },
    brown: { hue: 38, sat: 40, light: 35, alpha: 1 },
    lightbrown: { hue: 38, sat: 22, light: 52, alpha: 1 },
    yellow: { hue: 46, sat: 70, light: 52, alpha: 1 },
    lightyellow: { hue: 46, sat: 62, light: 70, alpha: 1 },
    darkgreen: { hue: 140, sat: 95, light: 22, alpha: 1 },
    green: { hue: 140, sat: 52, light: 37, alpha: 1 },
    lightgreen: { hue: 140, sat: 32, light: 58, alpha: 1 },
    darkcyan: { hue: 192, sat: 90, light: 26, alpha: 1 },
    cyan: { hue: 192, sat: 50, light: 46, alpha: 1 },
    lightcyan: { hue: 192, sat: 35, light: 60, alpha: 1 },
    darkblue: { hue: 210, sat: 90, light: 30, alpha: 1 },
    blue: { hue: 210, sat: 70, light: 44, alpha: 1 },
    lightblue: { hue: 210, sat: 55, light: 60, alpha: 1 },
    darkindigo: { hue: 240, sat: 88, light: 30, alpha: 1 },
    indigo: { hue: 240, sat: 42, light: 52, alpha: 1 },
    lightindigo: { hue: 240, sat: 42, light: 62, alpha: 1 },
    darkpurple: { hue: 295, sat: 75, light: 30, alpha: 1 },
    purple: { hue: 295, sat: 46, light: 42, alpha: 1 },
    lightpurple: { hue: 295, sat: 30, light: 54, alpha: 1 },
  },

});







/** 
 * ПЛАГИН ДЛЯ СТОЛБЦА ТАБЛИЦЫ ДАННЫХ С РЕДАКТОРОМ-КОМБОБОКСОМ.
 * ВЫПОЛНЯЕТ АВТОМАТИЧЕСКОЕ ОБНОВЛЕНИЕ РЕДАКТОРА-КОМБОБОКСА
 * МАССИВОМ УНИКАЛЬНЫХ ЗНАЧЕНИЙ-ОПЦИЙ, ИЗВЛЕЧЕННЫХ ИЗ ХРАНИЛИЩА ДАННЫХ ТАБЛИЦЫ.
 */
Ext.define('ZML.plugin.ColumnComboEditor', {

  extend: 'Ext.AbstractPlugin',
  alias: 'plugin.ColumnComboEditor',


  /** Источник данных для инлайн-комбобокса */
  DataSrc: null,

  /** 
   * Поле в источнике данных (либо в модели данных таблицы), выступающее в роли значимого поля для комбобокса.
   * В случае, если это свойство равно null, значимое и отображаемое поля комбобокса будет совпадать.
   */
  editorValueField: null,

  editorInited: false,





  /**
   * Выполняет инициализацию плагина.
   * @param {any} comp Столбец таблицы данных, к которому подключен этот плагин.
   */
  init: function (comp)
  {
    var me = this;
    comp.COMBO_EDITOR_PLUGIN = me;

    comp.on('afterrender', me.OnColumnAfterRender, { delay: 1000 });

    me.callParent(arguments);
  },





  /**
   * Метод, вызываемый в качестве обработчика события afterrender, сгенерированного
   * столбцом таблицы данных, к которому подключен этот плагин.
   * @param {any} comp Столбец таблицы данных, к которому подключен этот плагин.
   */
  OnColumnAfterRender: function (comp)
  {
    var me = comp.COMBO_EDITOR_PLUGIN;
    var grid = comp.up('grid') || comp.up('treepanel');
    var store = grid.getStore();

    if (store)
    {
      store.on('load', me.ComputeComboEditorData, me);
    };
    if (me.DataSrc && Ext.getStore(me.DataSrc))
    {
      Ext.getStore(me.DataSrc).on('load', me.ComputeComboEditorData, me);
      Ext.getStore(me.DataSrc).load();
    };
  },





  /**
   * Извлекает массив уникальных значений из записей в указанном хранилище данных.
   * Значения из записей выбираются по полю, имя которого соответствует
   * свойству dataIndex столбца, к которому подключен этот плагин.
   * Обновляет внутреннее хранилище редактора-комбобокса в этом столбце найденными значениями.
   * @param {any} stor Экземпляр хранилища данных - поставщика записей для извлечения значений.
   */
  ComputeComboEditorData: function (stor)
  {
    var me = this;

    if (me.DataSrc && Ext.getStore(me.DataSrc))
    {
      stor = Ext.getStore(me.DataSrc);
    };

    var uniqueArray = Ext.Array.unique(stor.data.items.map(function (m)
    {
      if (me.editorValueField)
      {
        var retobj = {};
        retobj[me.editorValueField] = m.get(me.editorValueField);
        retobj[me.cmp.dataIndex] = m.get(me.cmp.dataIndex);
        return retobj;
      }
      else
      {
        return m.get(me.cmp.dataIndex);
      };
    }));
    var sorted = Ext.Array.sort(uniqueArray);

    if (me.editorValueField)
    {
      var ret = sorted.map(function (m)
      {
        var obj = {};
        obj[me.editorValueField] = m[me.editorValueField];
        obj[me.cmp.dataIndex] = m[me.cmp.dataIndex];
        return obj;
      });
    }
    else
    {
      var ret = sorted.map(function (m)
      {
        var obj = {};
        obj[me.cmp.dataIndex] = m;
        return obj;
      });
    };

    me.SetComboEditorData(ret);
  },

  /**
   * Обновляет внутреннее хранилище редактора-комбобокса в столбце, 
   * к которому подключен этот плагин, передавая в хранилище указанный массив значений.
   * @param {any} data Массив значений - опций для редактора-комбобокса.
   */
  SetComboEditorData: function (data)
  {
    var me = this;
    var editor = me.cmp.getEditor();
    var grid = me.cmp.up('grid') || me.cmp.up('treepanel');
    var store = grid.getStore();


    //if (me.DataSrc && Ext.getStore(me.DataSrc))
    //{
    //  editor.bindStore(Ext.getStore(me.DataSrc));
    //}
    //else

    if (editor && editor.is('combobox'))
    {
      var fields = me.editorValueField ? [{ name: me.editorValueField, type: 'auto' }, { name: me.cmp.dataIndex, type: 'auto' }] : [{ name: me.cmp.dataIndex, type: 'auto' }]

      var modelname = grid.itemId + '-' + me.cmp.dataIndex + '-EditorModel';
      Ext.define(modelname, {
        extend: 'Ext.data.Model',
        idProperty: me.editorValueField || me.cmp.dataIndex,
        fields: fields
      });

      var cmbstor = Ext.create('Ext.data.Store', {
        model: modelname,
        combo: editor,
        data: [].concat(data)
      });

      cmbstor.on('load', function (stor)
      {
        var emptytext = '' + stor.getCount() + ' элем.';
        stor.combo.emptyText = emptytext;
        stor.combo.applyEmptyText();
      });

      editor.bindStore(cmbstor);

      editor.on('expand', me.SelectValues, me);
    };
  },


  SelectValues: function ()
  {
    var me = this;
    var editor = me.cmp.getEditor();
    //var grid = editor.column.up('grid');
    //var ctx = grid.getPlugin('cedit').context;

    if (editor && editor.is('combobox'))
    {
      if (editor.multiSelect == true)
      {
        var valuesArray = editor.rawValue.split(editor.delimiter);
        var arr = valuesArray.map(function (m) { return editor.getStore().findRecord(me.editorValueField || me.cmp.dataIndex, m); });
        editor.select(arr);
      }
      else
      {
        var record = editor.getStore().findRecord(me.editorValueField || me.cmp.dataIndex, editor.getValue());

        editor.select(record);
      };
    };
  },

});




/**
 * ПЛАГИН, ДОБАВЛЯЮЩИЙ РАСШИРЕННЫЕ ВОЗМОЖНОСТИ
 * РЕДАКТИРОВАНИЯ ТАБЛИЦЫ ДАННЫХ.
 */
Ext.define('ZML.plugin.GridEditor', {

  extend: 'Ext.AbstractPlugin',
  alias: 'plugin.GridEditor',


  mixins: {
    observable: 'Ext.util.Observable',
  },



  /** Содержимое буфера обмена, вставленное в подключенную таблицу данных */
  CLIPBOARD: '',

  /** Меню плагина */
  MENU: null,





  init: function (comp)
  {
    var me = this;
    comp.GRID_EDITOR = me;

    comp.on('afterrender', function (obj)
    {
      var compid = obj.getId();
      document.getElementById(compid).addEventListener('paste', obj.GRID_EDITOR.HandlePaste);
    });

    me.callParent(arguments);

    me.mixins.observable.constructor.call(me, me);
    me.addEvents('pasteCompleted');
    me.on({
      pasteCompleted: me.OnPasteCompleted,
      scope: me
    });
  },



  /**
   * Обработчик события pasteCompleted, генерируемого плагином
   * после успешной записи содержимого буфера обмена в свойство CLIPBOARD.
   * @param {any} obj Экземпляр плагина, сгенерировавший это событие.
   */
  OnPasteCompleted: function (obj)
  {
    var rowsList = obj.CLIPBOARD.trim().split('\n');
    var columns = obj.cmp.initialConfig.columns;
    var stor = obj.cmp.getStore();
    var model = stor.model.getName();
    var key = stor.KEY || stor.first().idField.name;
    var dataIndices = columns.items.map(function (m) { return m.dataIndex; });

    rowsList.forEach(function (r)
    {
      var newRow = obj.CreateNewRow(stor, model);

      r.trim().split('\t').forEach(function (f, i)
      {
        var type = stor.model.getFields().find(function (ff) { return ff.name == dataIndices[i]; }).type.type;

        switch (type)
        {
          case 'date': var res = Ext.Date.parse(f, 'd.m.Y'); break;
          case 'float': var res = parseFloat(f.replace(',', '.')); break;
          case 'int': var res = parseInt(f); break;
          case 'boolean': var res = (f == '0' || f.toString().toLowerCase() == 'false') ? false : true; break;
          case 'string': var res = f; break;


          default: var res = f; break;
        };

        newRow.set(dataIndices[i], res);
      });

      stor.add(newRow);
    });

    var updButton = obj.cmp.PAGING.UpdateButton;
    updButton.setDisabled(false);

  },





  /**
   * Создает одну либо несколько новых записей и добавляет их в хранилище данных
   * @param {any} amount Количество новых записей, создаваемых при запуске метода
   */
  PrepareNewRows: function (amount)
  {
    var me = this;
    var store = me.cmp.getStore();
    var model = store.model.getName();
    var key = store.KEY || store.first().idField.name;

    amount = amount || 1;

    if (model)
    {
      var tempRecordObject = {};
      var prmlinkers = store.paramLinkers;
      prmlinkers.forEach(function (p)
      {
        tempRecordObject[p.as] = p.value;
      });

      for (var i = 1; i <= amount; i++)
      {
        var lastDirtyKey = store.last() ? store.last().getId() : 0;
        var newDirtyKey = lastDirtyKey < -1 ? (lastDirtyKey - 1) : -2;

        var newRecordObject = Ext.apply({}, tempRecordObject);
        newRecordObject[key] = newDirtyKey;

        var newRecord = Ext.create(model, newRecordObject);
        store.add([newRecord]);
      };

    };
  },



  CreateNewRow: function (store, model)
  {
    if (store && model)
    {
      var key = store.KEY || store.first().idField.name;

      var tempRecordObject = {};
      var prmlinkers = store.paramLinkers;
      prmlinkers.forEach(function (p)
      {
        tempRecordObject[p.as] = p.value;
      });

      var lastDirtyKey = store.last() ? store.last().getId() : 0;
      var newDirtyKey = lastDirtyKey < -1 ? (lastDirtyKey - 1) : -2;

      tempRecordObject[key] = newDirtyKey;
      var newRecord = Ext.create(model, tempRecordObject);

      return newRecord;
    };
  },


  /**
   * Получает данные из выделенных строк таблицы, конвертирует
   * их в представление, подходящее для массовой вставки
   * строк в таблицу Excel.
   * @param {any} withHeaders Если true, то данные будут сконвертированы вместе с заголовками столбцов
   */
  PrepareForExcel: function (withHeaders)
  {
    var me = this;
    var stor = me.cmp.getStore();
    var key = stor.KEY || stor.first().idField.name;
    var columns = me.cmp.initialConfig.columns;
    var dataIndices = columns.items.map(function (m) { return m.dataIndex; });

    var records = me.cmp.getSelectionModel().getSelection();

    var resultText = !!withHeaders ? dataIndices.join('\t') + '\n' : '';

    var res = records.map(function (rec)
    {
      var values = dataIndices.map(function (d)
      {
        var temp = rec.get(d);
        var result;
        if (typeof temp.toLocaleDateString === 'function')
        {
          result = temp.toLocaleDateString();
        }
        else if (!isNaN(parseFloat(temp)))
        {
          result = parseFloat(temp).toString().replace('.', ',');
        }
        else
        {
          result = temp;
        };
        return result;
      });
      return values.join('\t');
    });

    resultText += res.join('\n');

    return resultText;
  },


  /** Копирует в буфер обмена подготовленные для вставки в Excel данные */
  ExportToClipboard: function ()
  {
    var me = this;
    var selcount = me.cmp.getSelectionModel().getSelection().length;
    if (selcount > 0)
    {
      var resultText = me.PrepareForExcel(true);
      var ta = document.createElement('textarea');
      ta.id = 'tempcliparea';
      ta.style.position = 'absolute';
      ta.style.left = '-1000px';
      ta.style.top = '-1000px';
      ta.value = resultText;

      document.body.appendChild(ta);
      ta.focus();
      ta.select();

      document.execCommand('copy');

      setTimeout(function ()
      {
        document.body.removeChild(ta);
      }, 500);
    };
  },


  /**
   * Обработчик события paste, сгенерированного на подключенной к плагину таблице данных.
   * Перехватывает содержимое, вставленное в таблицу данных из буфера обмена
   * и записывает его в собственное свойство CLIPBOARD для дальнейшей обработки.
   * @param {any} ev Объект события paste
   */
  HandlePaste: function (ev)
  {
    var me = this;
    var grid = Ext.getCmp(me.id);

    var clipboardData;
    var pastedData;

    ev.stopPropagation();
    ev.preventDefault();

    clipboardData = ev.clipboardData || window.clipboardData;
    pastedData = clipboardData.getData('Text');

    grid.GRID_EDITOR.CLIPBOARD = pastedData;
    grid.GRID_EDITOR.fireEvent('pasteCompleted', grid.GRID_EDITOR);
    console.dir(pastedData);
  },

});




/**
 * ПЛАГИН, ПОДКЛЮЧАЕМЫЙ К ЭЛЕМЕНТУ РАЗМЕТКИ, 
 * ПОВЕДЕНИЕ КОТОРОГО ОПРЕДЕЛЯЕТСЯ СОБЫТИЯМИ ЕГО ХОСТ-ОБЪЕКТА
 * @alias HostContext
 */
Ext.define('ZML.plugin.HostContext', {

  extend: 'Ext.AbstractPlugin',
  alias: 'plugin.HostContext',


  mixins: {
    observable: 'Ext.util.Observable',
  },


  /** Ссылка на элемент разметки, к которому подключен этот плагин */
  COMP: null,
  /** Хранилище логов выполнения операций */
  LOG: [],
  /** Список конфигураторов привязки элемента к событию хост-объекта */
  CTX_BINDERS: [],




  /**
   * Инициализация плагина.
   * @param {any} comp Управляемый элемент разметки, к которому подключается этот плагин
   */
  init: function (comp)
  {
    var me = this;
    me.COMP = comp;

    me.callParent(arguments);

    me.mixins.observable.constructor.call(me, me);
    me.addEvents('executed');
    me.on('executed', me.OnExecuted, me);
  },





  OnExecuted: function (ctx, args)
  {
    this.addLogEntry(ctx, args);
    var specHandler = this.GetSpecificHandler(ctx, args);
    specHandler(this.COMP);
  },




  setCmp: function (comp)
  {
    var me = this;
    me.callParent(arguments);

    me.COMP = comp;
    var ctxbinders = [].concat(me.CTX_BINDERS);
    me.CTX_BINDERS = [];

    me.addCtxBinders(ctxbinders);

    me.COMP.CTX_BINDERS = me.CTX_BINDERS;
    me.COMP.HOST_CTX_PLUGIN = me;
  },


  addCtxBinders: function (binderConfigs)
  {
    var me = this;
    if (!Array.isArray(binderConfigs))
    {
      binderConfigs = Array.of(binderConfigs);
    };
    binderConfigs.forEach(function (bb)
    {
      bb['PLUGIN'] = me;
      var binder = Ext.create('ZML.ContextBinder', bb);
      if (binder.isContextBinder)
      {
        me.relayEvents(binder, ['executed']);
        me.CTX_BINDERS.push(binder);
      };
    });
  },


  addLogEntry: function (ctx, args)
  {
    var me = this;
    var entry = {
      ctx: ctx,
      args: args
    };
    me.LOG.push(entry);
  },


  /**
   * Возвращает метод-обработчик для указанного биндера.
   * @param {any} ctx
   * @param {any} args
   * @returns {() => {}}
   */
  GetSpecificHandler: function (ctx, args)
  {
    var me = this;
    var argums = Array.from(args);


    if (!!ctx && ctx.isContextBinder && Array.isArray(argums))
    {
      /// Если ХОСТ-объект, указанный в биндере, является формой ввода данных...
      if (ctx.HOST.is('form'))
      {
        return function (comp)
        {
          comp.setDisabled(!argums[1]);
        }
      };
      /// Если ХОСТ-объект, указанный в биндере, является комбобоксом...
      if (ctx.HOST.is('ComboBox'))
      {
        return function (comp)
        {
          if (ctx.hostEvent == 'availableChanged')
          {
            var state = ctx.HOST.isDisabled();
            comp.setDisabled(state);
          }
          else if (!!ctx.needSelection || !!me.COMP.needSelection)
          {
            var selcount = ctx.HOST.GetSelectedCount();
            var needsel = ctx.needSelection || comp.needSelection;
            switch (needsel)
            {
              case 'none': comp.setDisabled(false); break;
              case 'single': comp.setDisabled(selcount != 1); break;
              case 'multi': comp.setDisabled(selcount < 2); break;
              case 'any': comp.setDisabled(selcount == 0); break;

              default: comp.setDisabled(false); break;
            };
          };
        };
      };
      /// Если ХОСТ-объект, указанный в биндере, является таблицей данных...
      if (ctx.HOST.is('grid') && (!!ctx.needSelection || !!me.COMP.needSelection))
      {
        return function (comp)
        {
          var selcount = argums[1].length;
          var needsel = ctx.needSelection || comp.needSelection;
          switch (needsel)
          {
            case 'none': comp.setDisabled(false); break;
            case 'single': comp.setDisabled(selcount != 1); break;
            case 'multi': comp.setDisabled(selcount < 2); break;
            case 'any': comp.setDisabled(selcount == 0); break;

            default: comp.setDisabled(false); break;
          };
        };
      };
      /// Если ХОСТ-объект, указанный в биндере, является списком данных...
      if (ctx.HOST.is('List') && (!!ctx.needSelection || !!me.COMP.needSelection))
      {
        return function (comp)
        {
          var selcount = argums[2].length;
          var needsel = ctx.needSelection || comp.needSelection;
          switch (needsel)
          {
            case 'none': comp.setDisabled(false); break;
            case 'single': comp.setDisabled(selcount != 1); break;
            case 'multi': comp.setDisabled(selcount < 2); break;
            case 'any': comp.setDisabled(selcount == 0); break;

            default: comp.setDisabled(false); break;
          };
        };
      };
      /// Если ХОСТ-объект, указанный в биндере, является таблицей с древовидным отображением данных...
      if (ctx.HOST.is('Tree') && (!!ctx.needSelection || !!me.COMP.needSelection))
      {
        return function (comp)
        {
          var selcount = argums[1].length;
          var needsel = ctx.needSelection || comp.needSelection;
          switch (needsel)
          {
            case 'none': comp.setDisabled(false); break;
            case 'single': comp.setDisabled(selcount != 1); break;
            case 'multi': comp.setDisabled(selcount < 2); break;
            case 'any': comp.setDisabled(selcount == 0); break;

            default: comp.setDisabled(false); break;
          };
        };
      };
      /// Если ХОСТ-объект, указанный в биндере, является полем ввода данных, либо элементом KeyValueInput/PairedCombo...
      if (ctx.HOST.is('field') || ctx.HOST.is('KeyValueInput') || ctx.HOST.is('PairedCombo'))
      {
        return function (comp)
        {
          comp.setDisabled(!argums[1]);
        }
      };

    };
  },

});










Ext.define('ZML.plugin.ListFilteredColumn', {

  extend: 'Ext.AbstractPlugin',
  alias: 'plugin.ListFilteredColumn',



  init: function (comp)
  {
    var me = this;
    comp.LIST_FILTER = me;
    me.callParent(arguments);
    comp.on('afterrender', function ()
    {
      var grid = comp.up('grid') || comp.up('treepanel');
      var store = grid.getStore();
      store.on('load', me.computeOptions, me);
    });
  },



  computeOptions: function (stor)
  {
    var me = this;
    var uniqueArray = Ext.Array.unique(stor.data.items.map(function (m)
    {
      return m.get(me.cmp.dataIndex);
    }));

    me.processOptions(stor, uniqueArray);

    var grid = me.cmp.up('grid') || me.cmp.up('treepanel');
    grid.filters.createFilters();
  },



  processOptions: function (stor, values)
  {
    var me = this;
    if (me.cmp.filter != undefined && Array.isArray(me.cmp.filter.options))
    {
      me.cmp.filter.options = [].concat(values);
    }
    else
    {
      me.cmp.filter = {
        type: 'list',
        idField: me.cmp.dataIndex,
        dataIndex: me.cmp.dataIndex,
        labelField: me.cmp.dataIndex,
        phpMode: true,
        options: [].concat(values)
      };
    };
  },

});




Ext.define('ZML.plugin.MonthFilteredColumn', {

  extend: 'Ext.AbstractPlugin',
  alias: 'plugin.MonthFilteredColumn',


  init: function (comp)
  {
    var me = this;
    comp.MONTH_FILTER = me;
    me.callParent(arguments);
    comp.on('afterrender', function ()
    {
      me.processOptions();
    });
  },


  processOptions: function ()
  {
    var me = this;
    me.cmp.filter = {
      type: 'month',
      dataIndex: me.cmp.dataIndex
    };
  },


});




Ext.define('ZML.plugin.Ripple', {

  extend: 'Ext.AbstractPlugin',
  alias: 'plugin.Ripple',



  autoCenter: false,

  active: false,

  pressed: false,

  noTap: false,

  color: null,

  density: 'light',

  rippleTs: 0,




  init: function (comp)
  {
    var me = this;
    comp.RIPPLE = me;

    var tpl = document.createElement('div');
    tpl.classList.add('x-ripple');
    tpl.setAttribute('no-tap', me.noTap);
    var ripground = document.createElement('div');
    ripground.classList.add('ripple-ground');
    ripground.setAttribute('hidden', me.autoCenter);
    ripground.setAttribute('active', me.active);
    var ripwaves = document.createElement('div');
    ripwaves.classList.add('ripple-waves');
    tpl.appendChild(ripground);
    tpl.appendChild(ripwaves);

    comp.on('render', function ()
    {
      comp.el.dom.appendChild(tpl);
      comp.on('click', function (obj, evt)
      {
        var arr = evt.getXY();
        comp.RIPPLE.pressed = true;
        (comp.RIPPLE.pressed) ? comp.RIPPLE.ripple(arr[0], arr[1]) : comp.RIPPLE.smooth();
        comp.RIPPLE.pressed = false;
      });
    })

  },









  ripple: function (startX, startY)
  {
    var me = this;
    var dom = me.cmp.el.dom;
    if (dom.querySelector('.ripple-waves') === null) return;

    var ripwaves = dom.querySelector('.ripple-waves');
    var ripground = dom.querySelector('.ripple-ground');
    var wave = ripwaves.appendChild(document.createElement('div'));
    var boundings = dom.querySelector('.x-ripple').getBoundingClientRect();
    var centerX = (startX === undefined || startX === null || Number.isNaN(startX)) || !ripground || ripground.hasAttribute('hidden');
    var centerY = (startY === undefined || startY === null || Number.isNaN(startY)) || !ripground || ripground.hasAttribute('hidden');
    var data = {
      radius: 0,
      left: 0,
      top: 0,
      dx: 0,
      dy: 0
    };

    me.rippleTs = Date.now();
    dom.querySelector('.x-ripple').classList.add('rippling');

    data.radius = Math.floor(Math.max(boundings.width, boundings.height)) * 1.5;

    var boundLeft = boundings.left;
    var boundTop = boundings.top;
    var halfRadius = (data.radius / 2);
    var halfBoundWidth = boundings.width / 2;
    var halfBoundHeight = boundings.height / 2;

    data.left = centerX ? (halfBoundWidth) : Math.max(startX, boundLeft) - Math.min(startX, boundLeft);
    data.top = centerY ? (halfBoundHeight) : Math.max(startY, boundTop) - Math.min(startY, boundTop);

    data.left -= halfRadius;
    data.top -= halfRadius;

    data.dx = halfBoundWidth - data.left - halfRadius;
    data.dy = halfBoundHeight - data.top - halfRadius;

    wave.style.height = data.radius + 'px';
    wave.style.top = data.top + 'px';
    wave.style.left = data.left + 'px';
    wave.style.width = data.radius + 'px';

    if (ripground) ripground.classList.add('rippling');

    ripwaves.classList.add('rippling');
    wave.classList.add('rippling');

    requestAnimationFrame(function ()
    {
      wave.style.transform = 'translate(' + data.dx + 'px, ' + data.dy + 'px) scale(1)';
      if (!me.pressed) me.smooth();
    });
  },


  smooth: function (force)
  {
    var me = this;
    var dom = me.cmp.el.dom;
    if (dom.querySelector('.ripple-waves') === null) return;

    var ripwaves = dom.querySelector('.ripple-waves');
    var ripground = dom.querySelector('.ripple-ground');
    var elapsed = Date.now() - me.rippleTs;
    var wave = ripwaves.querySelector('.rippling');
    var last = ripwaves.querySelectorAll('.rippling').length < 2;

    if (!wave) return;

    if (!force && elapsed < 400)
    {
      setTimeout(function () { me.smooth(true) }, 420 - elapsed);
      return;
    }

    me.pressed = false;

    if (ripground && last) ripground.classList.remove('rippling');

    wave.classList.remove('rippling');
    wave.classList.add('smoothing');

    setTimeout(function ()
    {
      ripwaves.removeChild(wave) && (ripwaves.children.length || ripwaves.classList.remove('rippling'));
    }, 750);

    dom.querySelector('.x-ripple').classList.remove('rippling');
  },


});









/** КОНФИГУРАТОР ПРИВЯЗКИ УПРАВЛЯЕМОГО ЭЛЕМЕНТА РАЗМЕТКИ К СВОЕМУ ХОСТ-ОБЪЕКТУ */
Ext.define('ZML.ContextBinder', {

  extend: 'Ext.Component',


  mixins: {
    observable: 'Ext.util.Observable',
  },


  /** Хост-объект, обнаруженный текущей реализацией метода hostLocator */
  HOST: null,
  /** Плагин, подключенный к управляемому элементу разметки, в котором определена эта привязка */
  PLUGIN: null,
  /** Сводка о работе этой привязки */
  Summary: {
    callsCount: 0,
    lastResult: [],
  },



  config: {
    /** Тип хост-объекта */
    hostType: null,
    /** 
     * Идентификатор хост-объекта.
     * Может быть равным null, если управляемый элемент находится непосредственно
     * внутри своего хост-объекта. В этом случае поиск хост-объекта 
     * будет выполняться через this.up(hostType) и будет невозможен
     * для невизуальных хост-объектов типа source и т.п. 
     */
    hostId: null,
    /** Событие, генерируемое хост-объектом, определяющее поведение управляемого элемента разметки */
    hostEvent: null,
    /** Предыдущее значение свойства hostEvent (до его замены на текущее) */
    lastHostEvent: null,
    /** Признак наличия сконфигурированного хост-объекта (факт того, что хост-объект найден и сконфигурирован) */
    isHostConfigured: false,
    /** Признак отключенной привязки, отсоединенной от своих плагинов/элементов/объектов */
    isDetached: false,
    /** Признак того, что данный объект является экземпляром своего класса */
    isContextBinder: true,

    needSelection: null
  },



  /**
   * Сеттер для свойства hostType. 
   * Генерирует событие hostTypeChanged при изменении свойства.
   * @param {any} value
   */
  setHostType: function (value)
  {
    var oldvalue = this.getHostType();
    this.hostType = value;
    if (oldvalue !== value)
    {
      this.fireEvent('hostTypeChanged', this, oldvalue, value);
    };
  },

  /**
   * Сеттер для свойства hostId. 
   * Генерирует событие hostIdChanged при изменении свойства.
   * @param {any} value
   */
  setHostId: function (value)
  {
    var oldvalue = this.getHostId();
    this.hostId = value;
    if (oldvalue !== value)
    {
      this.fireEvent('hostIdChanged', this, oldvalue, value);
    };
  },

  /**
   * Сеттер для свойства hostEvent. 
   * Генерирует событие hostEventChanged при изменении свойства.
   * @param {any} value
   */
  setHostEvent: function (value)
  {
    var me = this;
    me.lastHostEvent = me.getHostEvent();
    me.hostEvent = value;
    if (me.getLastHostEvent() != value)
    {
      me.fireEvent('hostEventChanged', me, me.lastHostEvent, value);
    };
  },





  initComponent: function (config)
  {
    var me = this;
    me.callParent(arguments);
    me.initConfig(config);

    me.mixins.observable.constructor.call(me, config);
    me.addEvents('created', 'hostTypeChanged', 'hostIdChanged', 'hostEventChanged', 'hostLocated', 'hostConfigured', 'executed');

    me.on({
      created: me.OnCreated,
      hostTypeChanged: me.OnHostTypeChanged,
      hostIdChanged: me.OnHostIdChanged,
      hostEventChanged: me.OnHostEventChanged,
      hostLocated: me.OnHostLocated,
      hostConfigured: me.OnHostConfigured,
      executed: me.OnExecuted,
      beforedestroy: me.OnBeforeDestroy,
      destroy: me.OnDestroy,
      scope: me
    });

    me.fireEvent('created', me);
  },




  /** 
   * Заглушка для динамически конструируемого метода, 
   * выполняющего поиск хост-объекта по его текущим
   * параметрам, заданным в свойствах hostType, hostId.
   */
  hostLocator: null,

  /**
   * Выполняет первый поиск хост-объекта и первое создание
   * динамического метода hostLocator.
   * Выполняется только в случае, если свойство hostType
   * содержит значение, И метод hostLocator еще не инициализирован.
   * После выполнения этих действий запускает метод tryLocateHost.
   */
  initHostLocator: function ()
  {
    var me = this;
    if (me && !!me.hostType && me.hostLocator === null)
    {
      var elem = me.PLUGIN.COMP;

      switch (me.hostType)
      {

        /// Если искомый хост-объект - источник данных...
        case 'source':
        case 'Source':
          me.hostLocator = function ()
          {
            return Ext.getStore(me.hostId);
          };
          break;


        /// Если искомый хост-объект - список с данными...
        case 'List':
          me.hostLocator = function ()
          {
            /// Проверяем принципиальную возможность выполнения поиска типа this.up(...)
            var hasUpMethod = !!me.PLUGIN && !!me.PLUGIN.cmp && typeof me.PLUGIN.cmp.up === 'function';

            if (!!me.getHostId())
            {
              return GLOBAL[elem.AppNAME].List[me.getHostId()];
            }
            else if (hasUpMethod)
            {
              return me.PLUGIN.cmp.up('List');
            };
          };
          break;


        /// Если искомый хост-объект - дерево узлов...
        case 'Tree':
          me.hostLocator = function ()
          {
            /// Проверяем принципиальную возможность выполнения поиска типа this.up(...)
            var hasUpMethod = !!me.PLUGIN && !!me.PLUGIN.cmp && typeof me.PLUGIN.cmp.up === 'function';

            if (!!me.getHostId())
            {
              return GLOBAL[elem.AppNAME].Tree[me.getHostId()];
            }
            else if (hasUpMethod)
            {
              return me.PLUGIN.cmp.up('Tree');
            };
          };
          break;



        /// Если искомый хост-объект - поле ввода данных...
        case 'Input':
          me.hostLocator = function ()
          {
            return GLOBAL[elem.AppNAME].Input[me.getHostId()];
          };
          break;



        /// Если искомый хост-объект - комбобокс...
        case 'Combo':
          me.hostLocator = function ()
          {
            return GLOBAL[elem.AppNAME].Combo[me.getHostId()];
          };
          break;



        /// Если искомый хост-объект - форма ввода данных...
        case 'Form':
          me.hostLocator = function ()
          {
            return GLOBAL[elem.AppNAME].Form[me.getHostId()];
          };
          break;



        /// Если искомый хост-объект - редактор исходного кода...
        case 'Editor':
          me.hostLocator = function ()
          {
            return GLOBAL[elem.AppNAME].Editor[me.getHostId()];
          };
          break;





        /// Для всех прочих типов хост-объектов...
        default:
          me.hostLocator = function ()
          {
            /// Проверяем принципиальную возможность выполнения поиска типа this.up(...)
            var hasUpMethod = !!me.PLUGIN && !!me.PLUGIN.cmp && typeof me.PLUGIN.cmp.up === 'function';

            if (!!me.getHostId())
            {
              return GLOBAL[elem.AppNAME][me.hostType][me.getHostId()];
            }
            else if (hasUpMethod)
            {
              return me.PLUGIN.cmp.up(me.getHostType());
            };
          };
          break;
      };

      setTimeout(function () { me.tryLocateHost(); }, 0);
    };
  },

  /**
   * Запускает метод hostLocator и записывает 
   * результаты его выполнения в свойство HOST.
   * После выполнения этих действий генерирует событие hostLocated.
   * Выполняется только в случае, если метод hostLocator 
   * уже проинициализирован и представляет из себя функцию.
   */
  tryLocateHost: function ()
  {
    var me = this;
    if (me.hostLocator !== null && typeof me.hostLocator === 'function')
    {
      me.HOST = me.hostLocator();
      setTimeout(function ()
      {
        if (!!me.HOST)
        {
          me.fireEvent('hostLocated', me);
        };
      }, 0)
    };
  },

  /**
   * Выполняет (ре)конфигурирование найденного хост-объекта:
   * удаляет из него имеющийся обработчик предыдущего (неактуального более) события hostEvent
   * и устанавливает новый обработчик текущего (актуального) события hostEvent.
   * В качестве метода-обработчика события хост-объекта используется локальный метод notifyContextManager.
   * Метод tryConfigureHost выполняется только в случае, если новое (актуальное) 
   * событие hostEvent не является falsy и хост-объект найден.
   * После выполнения всех описанных операций генерирует событие hostConfigured.
   * @param {any} oldevent Предыдущее событие hostEvent для удаления неактуального обработчика
   * @param {any} newevent Текущее событие hostEvent для установки актуального обработчика
   */
  tryConfigureHost: function (oldevent, newevent)
  {
    var me = this;
    if (!!newevent && me.HOST && oldevent != newevent)
    {
      if (!!oldevent)
      {
        me.HOST.un(oldevent, me.notifyContextManager, me);
      };
      me.HOST.on(newevent, me.notifyContextManager, me);

      me.fireEvent('hostConfigured', me, oldevent, newevent);
    };
  },

  /** 
   * Метод, принимающий управление дальнейшими операциями при получении
   * управляющего события от хост-объекта.
   * Генерирует событие executed с аргументами, полученными от события хост-объекта.
   */
  notifyContextManager: function ()
  {
    var me = this;
    me.fireEvent('executed', me, arguments);
  },





  /**
   * Обработчик события created.
   * Запускает инициализацию метода hostLocator.
   * @param {any} ctx Конфигуратор привязки - экземпляр этого класса
   */
  OnCreated: function (ctx)
  {
    if (ctx && ctx.isContextBinder)
    {
      ctx.initHostLocator();
    };
  },

  /**
   * Обработчик события beforeDestroy.
   * @param {any} ctx Конфигуратор привязки - экземпляр этого класса
   */
  OnBeforeDestroy: function (ctx)
  {
    if (ctx && ctx.isContextBinder && ctx.HOST && !!ctx.hostEvent)
    {
      ctx.HOST.un(ctx.hostEvent, ctx.notifyContextManager, ctx);
      ctx.setIsDetached(true);
    };
    return true;
  },

  /**
   * Обработчик события destroy.
   * @param {any} ctx Конфигуратор привязки - экземпляр этого класса
   */
  OnDestroy: function (ctx)
  {
    console.dir(ctx.isDetached ? 'Successfully detached!' : 'Detach failed!');
  },

  /**
   * Обработчик события hostTypeChanged.
   * Выполняет сброс и повторную инициализацию метода hostLocator в случае,
   * если новое значение свойства hostType не является falsy.
   * @param {any} ctx Конфигуратор привязки - экземпляр этого класса
   * @param {any} oldvalue Предыдущее значение свойства hostType
   * @param {any} newvalue Текущее значение свойства hostType
   */
  OnHostTypeChanged: function (ctx, oldvalue, newvalue)
  {
    if (ctx && ctx.isContextBinder)
    {
      if (oldvalue != newvalue && !!newvalue)
      {
        ctx.hostLocator = null;
        ctx.initHostLocator();
      };
    };
  },

  /**
   * Обработчик события hostIdChanged.
   * Запускает повторный поиск хост-объекта вызовом метода tryLocateHost в случае,
   * если новое значение свойства hostId не является falsy.
   * @param {any} ctx Конфигуратор привязки - экземпляр этого класса
   * @param {any} oldvalue Предыдущее значение свойства hostId
   * @param {any} newvalue Текущее значение свойства hostId
   */
  OnHostIdChanged: function (ctx, oldvalue, newvalue)
  {
    if (ctx && ctx.isContextBinder)
    {
      if (oldvalue != newvalue && !!newvalue)
      {
        ctx.tryLocateHost();
      };
    };
  },

  /**
   * Обработчик события hostEventChanged.
   * Запускает реконфигурацию хост-объекта вызовом метода tryConfigureHost.
   * @param {any} ctx Конфигуратор привязки - экземпляр этого класса
   * @param {any} oldvalue Предыдущее значение свойства hostEvent
   * @param {any} newvalue Текущее значение свойства hostEvent
   */
  OnHostEventChanged: function (ctx, oldvalue, newvalue)
  {
    if (ctx && ctx.isContextBinder)
    {
      ctx.tryConfigureHost(oldvalue, newvalue);
    };
  },

  /**
   * Обработчик события hostLocated.
   * Запускает реконфигурацию хост-объекта вызовом метода tryConfigureHost,
   * используя текущие значения свойств lastHostEvent и hostEvent как параметры его вызова
   * в качестве предыдущего и текущего значения свойства hostEvent соответственно.
   * @param {any} ctx Конфигуратор привязки - экземпляр этого класса
   */
  OnHostLocated: function (ctx)
  {
    if (ctx && ctx.isContextBinder)
    {
      ctx.tryConfigureHost(ctx.lastHostEvent, ctx.hostEvent);
    };
  },

  /**
   * Обработчик события hostConfigured.
   * Устанавливает признак isHostConfigured равным true.
   * @param {any} ctx Конфигуратор привязки - экземпляр этого класса
   */
  OnHostConfigured: function (ctx)
  {
    if (ctx && ctx.isContextBinder)
    {
      ctx.setIsHostConfigured(true);
    };
  },

  /**
   * Обработчик события executed.
   * Увеличивает счетчик применения изменений к управляемому элементу на единицу,
   * записывает полученные от управляющего события хост-объекта аргументы в свойство Summary.lastResult.
   * Событие executed далее используется в плагине, подключенном к управляемому элементу.
   * @param {any} ctx Конфигуратор привязки - экземпляр этого класса
   * @param {any} args Аргументы управляющего события хост-объекта
   */
  OnExecuted: function (ctx, args)
  {
    if (ctx && ctx.isContextBinder)
    {
      ctx.Summary.callsCount++;
      ctx.Summary.lastResult = [].concat(args);
    };
  },

});
















/** 
 * ИСПОЛНЯЕМОЕ ДЕЙСТВИЕ 
 * Регистрация компонента производится при вызове метода initComponent.
 * Менеджер компонента: ZML.ActionMgr
 */
Ext.define('ZML.data.Action', {

  extend: 'Ext.Component',



  /** Локальный идентификатор действия. */
  itemId: null,

  /** Внешний идентификатор (имя) действия. */
  name: null,

  /** 
   * Тип запуска кода - локально/удаленно.
   * @type {"local"|"database"|"server"|"rest"|"signalr"}
   */
  runAt: 'local',


  url: null,

  method: null,


  /** Текст запроса на подтверждение выполнения этого действия. */
  confirmText: null,

  /** Текст заголовка информационного окна с запросом на подтверждение выполнения этого действия. */
  confirmTitle: 'Подтверждение действия',

  /** Описание действия. */
  description: null,


  /** Имя приложения (Ext.application({ name: AppNAME })) */
  AppNAME: 'APP',




  paramLinkers: [],







  initComponent: function ()
  {
    this.callParent(arguments);
    GLOBAL[this.AppNAME].Action.register(this);
  },








  /** Обработчик запроса на подтверждение выполнения этого действия. */
  Handler: function ()
  {
    var me = this;
    if (!!me.confirmText)
    {
      Ext.Msg.show({
        title: me.confirmTitle,
        msg: me.confirmText,
        buttons: Ext.Msg.YESNO,
        icon: Ext.Msg.QUESTION,
        cls: 'msgbox',
        fn: function (response)
        {
          if (response === 'yes')
          {
            me.Execute();
          };
        }
      });
    }
    else
    {
      me.Execute();
    };
  },

  /** Перенаправление на исполняемый метод. */
  Execute: function ()
  {
    switch (this.runAt)
    {
      case 'local': this.__Execute(); break;

      case 'database':
        if (this._foundAtStoreMgr())
        {
          var me = this;
          Ext.StoreManager.ACTION(this.itemId)[0].on('load', function (st, recs)
          {
            me._ProcessError(st, recs);
          });
          Ext.StoreManager.ACTION(this.itemId)[0].load()
        }
        else
        {
          this._showNotFoundAction();
        };
        break;

      case 'server': break;

      case 'rest': this.DoRequest(); break;

      case 'signalr': break;



      default: break;
    };
  },








  _getParamForRoute: function ()
  {
    var ret = this.paramLinkers.find(function (f) { return f.aspFrom === 'route' });
    return ret ? '/' + ret.value || '' : '';
  },


  _getParamsForQuery: function ()
  {
    var prms = this.paramLinkers.filter(function (f) { return f.aspFrom === 'query' && (!!f.as || !!f.field) });
    return prms.length > 0 ? '?' + prms.map(function (m)
    {
      if (Array.isArray(m.value))
      {
        return m.value.map(function (r) { return m.as + '=' + r }).join('&');
      }
      else
      {
        return m.as + '=' + m.value;
      };
    }).join('&') : '';
  },


  _getParamsForHeaders: function ()
  {
    var prms = this.paramLinkers.filter(function (f) { return f.aspFrom === 'header' && (!!f.as || !!f.field) });
    var headersObj = {};
    prms.forEach(function (f)
    {
      if (!Array.isArray(f.value) && typeof (f.value) === 'object')
      {
        Ext.apply(headersObj, f.value);
      }
      else
      {
        var tempObj = {};
        var key = f.as || f.field;
        tempObj[key] = f.value;
        Ext.apply(headersObj, tempObj);
      };
    });
    return prms.length > 0 ? headersObj : undefined;
  },


  _getParamForBody: function ()
  {
    var ret = this.paramLinkers.find(function (f) { return f.aspFrom === 'body' });
    return !!ret && this.method != 'GET' ? ret.value : null;
  },


  _getOtherParams: function ()
  {
    var prms = this.paramLinkers.filter(function (f) { return !f.aspFrom && (!!f.as || !!f.field) });
    var paramsObj = {};
    prms.forEach(function (f)
    {
      if (!Array.isArray(f.value) && typeof (f.value) === 'object')
      {
        Ext.apply(paramsObj, f.value);
      }
      else
      {
        var tempObj = {};
        var key = f.as || f.field;
        tempObj[key] = f.value;
        Ext.apply(paramsObj, tempObj);
      };
    });
    return prms.length > 0 ? paramsObj : undefined;
  },









  DoRequest: function ()
  {
    var me = this;
    me.paramLinkers.forEach(function (f) { f.compute(); });


    //var paramsObj = {};

    ////var hasQryStringParams = me.paramLinkers.filter(function (p) { return !!p.as || !!p.field; }).length > 0;

    //me.paramLinkers.filter(function (p) { return !!p.as || !!p.field; }).forEach(function (f)
    //{
    //  var tempObj = {};
    //  var key = f.as || f.field;
    //  tempObj[key] = f.value;
    //  Ext.apply(paramsObj, tempObj);
    //});

    ////var qryString = hasQryStringParams ? '?' + me.paramLinkers.filter(function (p) { return !!p.as || !!p.field; }).map(function (m) { return m.as + '=' + m.value }).join('&') : '';

    //var bodyParam = me.paramLinkers.find(function (f) { return !f.as || !f.field; });
    //var bodyRawData = !!bodyParam ? bodyParam.value : null;



    Ext.Ajax.request({
      url: me.url + me._getParamForRoute() + me._getParamsForQuery(),
      method: me.method,
      params: me._getOtherParams(),
      rawData: me._getParamForBody(),
      success: function (resp)
      {
        me.OnAfterRequest(resp.responseText);
      }
    });

  },



  /** 
   * Заглушка метода, непосредственно выполняющего это действие.
   * Эту функцию необходимо переопределить при создании экземпляра этого класса.
   */
  __Execute: function ()
  {
    console.dir(this.itemId + ': Не определена логика выполнения этого действия.')
  },



  /** 
   * Заглушка метода, выполняемого по нажатии на кнопку подтверждения в инфо-окне,
   * выводимом после выполнения этого действия.
   */
  OnConfirmAfterResponse: function ()
  {

  },

  /**
   * Заглушка метода, выполняемого по нажатии на кнопку отмены в инфо-окне,
   * выводимом после выполнения этого действия.
   */
  OnCancelAfterResponse: null,





  OnAfterRequest: function (data)
  {

  },







  /**
   * Отображает инфо-окно с сообщением об ошибке выполнения этого действия (для типа запуска = database).
   * Обрабатывает результаты взаимодействия пользователя с этим инфо-окном, 
   * запускает, в завимости от результата, один из двух методов (OnConfirmAfterResponse или OnCancelAfterResponse).
   * @param {any} title Заголовок инфо-окна.
   * @param {any} warningText Сообщение об ошибке, выводимое в инфо-окне.
   * @param {any} callback Опционально: пользовательский коллбэк, выполняемый вместо показа инфо-окна.
   */
  _ShowErrorMsgBox: function (title, warningText, callback)
  {
    var me = this;
    if (callback && typeof callback === 'function')
    {
      Ext.Msg.show({
        title: title,
        msg: warningText,
        buttons: Ext.Msg.OK,
        fn: callback
      });
    }
    else
    {
      if (me.OnCancelAfterResponse && typeof me.OnCancelAfterResponse === 'function')
      {
        Ext.Msg.show({
          title: title,
          msg: warningText,
          buttons: Ext.Msg.OKCANCEL,
          fn: function (btn, text, eventOptions, record)
          {
            switch (btn)
            {
              case 'ok':
                me.OnConfirmAfterResponse(warningText);
                break;

              case 'cancel':
                me.OnCancelAfterResponse();
                break;

              default:
                me.OnConfirmAfterResponse(warningText);
                break;
            };
          }
        });
      }
      else
      {
        Ext.Msg.show({
          title: title,
          msg: warningText,
          buttons: Ext.Msg.OK,
          fn: function (btn, text, eventOptions, record)
          {
            switch (btn)
            {
              case 'ok':
                me.OnConfirmAfterResponse(warningText);
                break;

              default:
                me.OnConfirmAfterResponse(warningText);
                break;
            };
          }
        });
      };
    };
  },

  /**
   * Обработчик ошибок выполнения этого действия (для типа запуска = database)
   * @param {any} stor Экземпляр хранилища данных, созданного для выполнения этого действия.
   * @param {any} recs Результат выполнения действия в виде набора записей.
   */
  _ProcessError: function (stor, recs)
  {
    var me = this;
    var first = recs[0];
    if (first && first.raw)
    {
      var iserror = first.raw.Error || first.raw.error;
      var errorHeader = first.get('ErrorHeader') || first.get('errorHeader') || 'Ошибка выполнения';
      if (errorText === undefined || errorText === '')
      {
        errorText = first.data.Error || first.data.error || first.get('error') || first.get('Error') || first.get('ErrorText') || first.get('errorText') || iserror;
      };
      if (!errorText)
      {
        return;
      };
      if (errorText != 'ok')
      {
        me._ShowErrorMsgBox(errorHeader, errorText);
        return;
      };
    }
    else
    {
      if (stor.proxy.reader.jsonData.errorText != 'ok')
      {
        me._ShowErrorMsgBox('Ошибка', errorText || stor.proxy.reader.jsonData.errorText);
        return;
      };
    };
  },






  /**
   * Возвращает true, если StoreManager содержит определение для этого действия
   * @private
   * */
  _foundAtStoreMgr: function ()
  {
    return !!Ext.StoreManager.ACTION(this.itemId)[0];
  },

  /**
   * Выводит информационное окно с сообщением об ошибке в случае, если
   * StoreManager не содержит определение для этого действия.
   * @private
   */
  _showNotFoundAction: function ()
  {
    var me = this;
    Ext.Msg.show({
      title: 'Действие не найдено',
      msg: 'StoreManager не содержит определения для действия с идентификатором ' + me.itemId,
      buttons: Ext.Msg.OK,
      icon: Ext.Msg.ERROR,
      cls: 'msgbox',
      fn: function (response)
      {
        if (response === 'ok')
        {
          return;
        };
      }
    });
  },

});




/**
 * ПАРАМЕТР
 * Задает источник получения данных для своего хост-объекта.
 * Поиск источника производится по его типу и идентификатору,
 * данные извлекаются из указанного свойства/содержимого источника.
 * 
 */
Ext.define('ZML.Param', {

  extend: 'Ext.Component',



  //scopeId: null,

  //scopeType: null,

  //field: null,

  //as: null,

  //value: undefined,

  //default: undefined,

  //record: 0,

  ///**
  // * Способ передачи параметра в ASP.NET Core контроллер.
  // * @type {'route'|'query'|'body'|'header'}
  // */
  //aspFrom: 'query',

  //isParam: null,

  //isCorrectScope: null,



  config: {

    scopeId: null,

    scopeType: null,

    field: null,

    as: null,

    value: undefined,

    default: undefined,

    record: 0,

    /**
     * Способ передачи параметра в ASP.NET Core контроллер.
     * @type {'route'|'query'|'body'|'header'}
     */
    aspFrom: 'query',

    isParam: null,

    isCorrectScope: null,

  },



  /** Имя приложения (Ext.application({ name: AppNAME })) */
  AppNAME: 'APP',




  initComponent: function (config)
  {
    this.callParent(arguments);
    config = config || {};
    var cfg = {
      scopeId: config.scopeId,
      scopeType: config.scopeType,
      field: config.field,
      as: config.as || config.field,
      record: config.record,
      value: config.value,
      default: config.value || config.default,
      aspFrom: config.aspFrom,
      isParam: true,
      isCorrectScope: this.IsCorrectScope(config.scopeType)
    };

    //me.scopeId = cfg.scopeId;
    //me.scopeType = cfg.scopeType;
    //me.field = cfg.field;
    //me.as = cfg.as;
    //me.record = cfg.record;
    //me.value = cfg.value;
    //me.default = cfg.default;
    //me.aspFrom = cfg.aspFrom;
    //me.isParam = cfg.isParam;
    //me.isCorrectScope = cfg.isCorrectScope;

    this.initConfig(cfg);
    //this.callParent([cfg]);
    //console.dir(cfg);
  },




  compute: function ()
  {
    var me = this;
    var ret;

    if (GLOBAL[me.AppNAME][me.scopeType] && typeof (GLOBAL[me.AppNAME][me.scopeType].EvaluateField) === 'function')
    {
      ret = GLOBAL[me.AppNAME][me.scopeType].EvaluateField(me.scopeId, me.field, me.record);
    };
    me.value = me.default == undefined ? ret : (typeof me.default === 'function' ? me.default.call(me) : me.default || me.getDefault());
    //me.value = me.getDefault() == undefined ? ret : (typeof me.getDefault() === 'function' ? me.getDefault().call(me) : me.getDefault());
  },



  IsCorrectScope: function (scope)
  {
    switch (scope)
    {
      //case 'Button':
      case 'Combo':
      //case 'Component':
      //case 'Dialog':
      case 'Editor':
      case 'Form':
      case 'Grid':
      case 'Input':
      case 'List':
      //case 'Panel':
      case 'Tabpanel':
      //case 'Template':
      case 'Tree':
        return true;

      default: return false;
    };
  },

});










/**
 * ЗАГРУЗЧИК СОДЕРЖИМОГО
 * @manager ZML.LoaderMgr
 * @registration initComponent
 */
Ext.define('ZML.data.Loader', {

  extend: 'Ext.ComponentLoader',




  itemId: null,

  paramLinkers: [],

  url: null,

  autoLoad: false,

  scripts: true,

  replaceContent: true,

  renderMode: 'component',



  /** Имя приложения (Ext.application({ name: AppNAME })) */
  AppNAME: 'APP',





  listeners: {
    beforeload: function (obj, options, eOpts)
    {
      obj.PrepareLoader();
      setTimeout(() =>
      {
        obj.OnBeforeLoad(obj, options, eOpts);
        obj.RemoveTargetContent();
      }, 0);
    },
    load: function (obj, response, options, eOpts)
    {
      if (obj.renderMode === 'markdown' && window['Prism'] && typeof (window['Prism'] === 'function'))
      {
        window.Prism.highlightAllUnder(obj.getTarget().el.dom);
      };
      obj.OnAfterLoad(obj, response, options, eOpts);
    }
  },



  initComponent: function ()
  {
    this.callParent(arguments);

    GLOBAL[this.AppNAME].Loader.register(this);
  },







  GetRenderMode: function ()
  {
    return this.renderMode;
  },


  GetUrl: function ()
  {
    return this.url;
  },


  SetUrl: function (url)
  {
    this.url = url;
  },



  Load: function ()
  {
    var me = this;
    if (me.getTarget())
    {
      me.load();
    };
  },





  ComputeParamLinkers: function ()
  {
    var me = this;
    me.params = {};
    me.paramLinkers.forEach(function (p)
    {
      p.compute();
      me.params[p.as] = p.value;
    });
  },



  PrepareLoader: function ()
  {
    var me = this;
    me.ComputeParamLinkers();

    me.__autoLoad = me.autoLoad;
    me.autoLoad = false;

    me.renderer = null;
    
    if (me.getTarget())
    {
      //me.autoLoad = me.__autoLoad;
      me.loadMask = { msg: 'Загружается...' };
      switch (me.renderMode)
      {
        case 'markdown': me.renderer = me.MarkdownRenderer; break;

        default: me.renderer = me.renderMode; break;
      };
    };    
  },





  MarkdownRenderer: function (loader, response, active)
  {
    if (window['markdownit'] && typeof (window['markdownit'] === 'function'))
    {
      var md = new window.markdownit('default', { html: true, typographer: true });
      var text = response.responseText;
      var result = md.render(text);
      loader.getTarget().addCls('markdown-body');
      loader.getTarget().update(result, true);

      return true;
    };
  },




  RemoveTargetContent: function ()
  {
    var me = this;
    if (typeof (me.getTarget) === 'function' && me.getTarget() && me.replaceContent === true)
    {
      me.getTarget().removeAll();
    };
  },


  UnlinkTarget: function ()
  {
    var me = this;
    if (me.getTarget())
    {
      me.getTarget().loader = null;
      me.setTarget(null);
    };
  },




  OnBeforeLoad: function ()
  {

  },


  OnAfterLoad: function ()
  {

  },

});










/** 
 * КНОПКА 
 * @alias Button
 * @manager ZML.ButtonMgr
 * @registration afterrender
 */
Ext.define('ZML.button.Button', {

  extend: 'Ext.button.Button',
  alias: 'widget.Button',

  targetId: null,
  call: null,
  iconSet: 'mat-icons',
  color: null,
  __icon: null,
  __cls: '',



  listeners: {
    render: function ()
    {
      var me = this;
      me.ComputeIcon();
      me.ComputeColor();
    },
    afterrender: function ()
    {
      GLOBAL[this.AppNAME].Button.register(this);
    }
  },



  /** Имя приложения (Ext.application({ name: AppNAME })) */
  AppNAME: 'APP',




  initComponent: function ()
  {
    this.callParent(arguments);

    //GLOBAL[this.AppNAME].Button.register(this);
    //window[this.AppNAME][this.AppPROP].MGR.Button.register(this);
  },



  handler: function (item, evt)
  {
    item.OnHandlerCall(item, evt);
    if (typeof this.ApiOnAfterClick === 'function')
    {
      Ext.Function.defer(this.ApiOnAfterClick, 0, this, arguments);
    };
  },


  ComputeIcon: function ()
  {
    var me = this;
    var spanImg = me.el.dom.querySelector('span[role=img]') || me.el.dom.querySelector('span.x-btn-icon-el');

    if (me.cls)
    {
      me.cls.split(' ').forEach(function (f) { me.removeCls(f); });
    };

    me.addCls(me.__cls);

    if (!me.text)
    {
      me.addCls('logx-icon-btn');
    }
    else
    {
      me.addCls('logx-small-btn');
    };

    if (!me.__icon)
    {
      me.addCls('no-icon');
    };
    if (me.iconSet)
    {
      me.addCls(me.iconSet);
    };
    if (me.__icon)
    {
      switch (me.iconSet)
      {
        case 'fa': me.setIconCls(me.__icon); break;
        case 'mat-icons': spanImg.textContent = me.__icon; break;

        default: break;
      };
    };
  },


  ComputeColor: function ()
  {
    var me = this;
    var spanImg = me.el.dom.querySelector('span[role=img]') || me.el.dom.querySelector('span.x-btn-icon-el');

    if (me.color)
    {
      if (me.color && me.color.startsWith('#'))
      {
        if (spanImg)
        {
          spanImg.style.color = me.color;
        };
      };
      if (/^[0-9]+.*%$/.test(me.color))
      {
        if (spanImg)
        {
          spanImg.style.color = 'hsla(' + me.color + ',1)';
        };
      };
      if (/^[0-9]+.*[^%]$/.test(me.color))
      {
        if (spanImg)
        {
          spanImg.style.color = 'hsla(' + me.color + ')';
        };
      };
      if (/^[a-z]+/.test(me.color))
      {
        if (me.color && me.color.startsWith('color-'))
        {
          me.setIconCls(me.iconCls + ' ' + me.color);
        }
        else
        {
          me.setIconCls(me.iconCls + ' color-' + me.color);
        };
      };
    };
  },


  OnHandlerCall: function (item, evt)
  {

  },


  ApiOnAfterClick: null,

});






/**
 * ДИАЛОГОВОЕ ОКНО
 * @alias Dialog
 * @manager ZML.DialogMgr
 * @registration initComponent
 */
Ext.define('ZML.dialog.Dialog', {

  extend: 'Ext.window.Window',
  alias: 'widget.Dialog',

  closeAction: 'hide',
  minWidth: 350,
  minHeight: 250,
  layout: 'fit',


  noRipple: false,

  standardButtons: true,

  ConfirmAction: null,

  ContextBinders: [],


  /** Имя приложения (Ext.application({ name: AppNAME })) */
  AppNAME: 'APP',




  listeners: {
    beforerender: function ()
    {
      if (!!this.standardButtons)
      {
        this.ComposeStandardButtons();
      };
    },
    show: function ()
    {
      this.OnDialogOpened();
    }
  },




  initComponent: function ()
  {
    this.callParent(arguments);

    GLOBAL[this.AppNAME].Dialog.register(this);
  },








  OnDialogOpened: function ()
  {

  },


  OnDialogCancelled: function ()
  {

  },


  OnDialogConfirmed: function ()
  {

  },






  /** Метод-компоновщик стандартных кнопок диалогового окна */
  ComposeStandardButtons: function ()
  {
    var me = this;
    if (!!me.standardButtons && !me.HasStdButtons())
    {
      var ctxplugin = me.ComposeContextPlugin('btnConfirm');
      var btnConfirmPlugins = me.noRipple ? [ctxplugin] : [ctxplugin, me.ComposeRipplePlugin('btnConfirm')];
      var btnCancelPlugins = me.noRipple ? [] : [me.ComposeRipplePlugin('btnCancel')];
      var firstWithNeedSel = me.ContextBinders.find(function (f) { return f.for === 'btnConfirm' && !!f.needSelection });
      var needSel = firstWithNeedSel ? firstWithNeedSel.needSelection : 'none';

      var confirmBtn = Ext.create('ZML.button.Button', {
        itemId: me.itemId + '-btnConfirm',
        call: 'action',
        disabled: true,
        text: 'ПРИМЕНИТЬ',
        iconSet: 'mat-icons',
        __icon: 'check_circle',
        color: 'darkgreen',
        needSelection: needSel,
        plugins: btnConfirmPlugins,
        OnHandlerCall: function ()
        {
          var dlg = this.up('window');
          if (dlg)
          {
            if (dlg.ConfirmAction) Ext.StoreManager.ACTION(dlg.ConfirmAction)[0].load();
            Ext.Function.defer(dlg.OnDialogConfirmed, 0, dlg);
            Ext.Function.defer(dlg.close, 350, dlg);
          };
        }
      });

      var cancelBtn = Ext.create('ZML.button.Button', {
        itemId: me.itemId + '-btnCancel',
        call: 'action',
        disabled: false,
        text: 'ОТМЕНИТЬ',
        iconSet: 'mat-icons',
        __icon: 'cancel',
        color: 'darkred',
        plugins: btnCancelPlugins,
        OnHandlerCall: function ()
        {
          var dlg = this.up('window');
          Ext.Function.defer(dlg.OnDialogCancelled, 0, dlg);
          Ext.Function.defer(dlg.close, 350, dlg);
        }
      });

      me.addDocked({ xtype: 'toolbar', dock: 'bottom', items: [{ xtype: 'tbfill' }, confirmBtn, cancelBtn] });
    };
  },


  ComposeContextPlugin: function (itemId)
  {
    var me = this;
    return {
      ptype: 'HostContext',
      pluginId: me.itemId + '-' + itemId + '-hostContextPlugin',
      CTX_BINDERS: me.ContextBinders.filter(function (f) { return f.for === itemId })
    };
  },


  ComposeRipplePlugin: function (itemId)
  {
    var me = this;
    return {
      ptype: 'Ripple',
      pluginId: me.itemId + '-' + itemId + '-ripplePlugin'
    };
  },






  HasStdButtons: function ()
  {
    var created = !!this.query('#' + this.itemId + '-btnConfirm')[0] && !!this.query('#' + this.itemId + '-btnCancel')[0];
    return !!this.standardButtons && created;
  },

});






/**
 * КОМБОБОКС
 * @alias ComboBox
 * @manager ZML.ComboMgr
 * @registration initComponent
 * @method GetSelectedValue @property value
 * @method GetTotalCount @property totalCount
 * @method setValue @property value
 * @method getDisplayValue @property displayValue
 * @method getDescriptionValue @property descrValue
 */
Ext.define('ZML.input.ComboBox', {

  extend: 'Ext.form.field.ComboBox',
  alias: 'widget.ComboBox',

  mixins: {
    handleClearIcon: 'ZML.mixin.HandleClearIcon',
    afterEventCallback: 'ZML.mixin.AfterEventCallback'
  },

  forceSelection: true,
  editable: false,
  enableKeyEvents: true,
  minChars: 2,
  queryMode: 'remote',
  flex: 1,
  columnWidth: 1,
  fieldCls: 'x-form-field basecombo',

  defaultValue: null,



  /** Имя приложения (Ext.application({ name: AppNAME })) */
  AppNAME: 'APP',




  TEMPLATE: [],






  initComponent: function ()
  {
    var me = this;
    me.callParent(arguments);

    me.addEvents('availableChanged');

    me.on({
      afterrender: me.OnAfterRender,
      change: me.OnChange,
      disable: me.OnDisabled,
      enable: me.OnEnabled,
      availableChanged: me.OnAvailableChanged,
      scope: me
    });

    me.inputElem = 'input';

    GLOBAL[this.AppNAME].Combo.register(me);
  },




  OnAfterRender: function (obj)
  {
    if (obj)
    {
      if (Array.isArray(obj.TEMPLATE) && obj.TEMPLATE.length)
      {
        obj.tpl = new Ext.XTemplate(obj.TEMPLATE);
      };

      setTimeout(function ()
      {
        obj.CountFoundItems();
        obj.SelectRecordByValue();
      }, 500);
    };
  },

  OnChange: function (obj)
  {
    if (obj)
    {
      obj.updateClearIcon();
      obj.CountFoundItems();
      //Ext.StoreManager.LoadLinkedStores({ scopeType: 'Combo', scopeId: obj.itemId })

      if ((obj.editable == true && obj.getSubmitValue().length >= obj.minChars) || obj.getSubmitValue().length == 0 || obj.editable == false)
      {
        Ext.StoreManager.LoadLinkedStores({ scopeType: 'Combo', scopeId: obj.itemId });
        obj.OnAfterEvent(obj.ApiOnChange, obj);
      };
    };
  },



  /**
   * Обработчик события disable.
   * @param {any} obj экземпляр комбобокса, сгенерировавшего событие
   */
  OnDisabled: function (obj)
  {
    if (!!obj)
    {
      obj.fireEvent('availableChanged', obj);
    };
  },

  /**
   * Обработчик события enable.
   * @param {any} obj экземпляр комбобокса, сгенерировавшего событие
   */
  OnEnabled: function (obj)
  {
    if (!!obj)
    {
      obj.fireEvent('availableChanged', obj);
    };
  },

  /**
   * 
   * @param {any} obj
   */
  OnAvailableChanged: function (obj)
  {

  },






  GetSelectedValue: function ()
  {
    var ret = this.getValue();
    if (ret == '' || ret == null)
    {
      ret = -1;
    };
    return ret; this.desc
  },


  SelectRecordByValue: function ()
  {
    var me = this;
    if (me.store && me.store.isStore)
    {
      var record = me.store.findRecord(me.valueField, me.defaultValue);
      me.select(record);
    };
  },


  CountFoundItems: function ()
  {
    var emptytext = '' + this.store.getCount() + ' элем.';
    this.emptyText = emptytext;
    this.applyEmptyText();
  },


  GetSelectedCount: function ()
  {
    return this.lastSelection.length;
  },


  GetTotalCount: function ()
  {
    var me = this;
    if (me.store && me.store.isStore)
    {
      return me.store.getCount();
    };
  },


  getDescriptionValue: function ()
  {
    var me = this;
    if (!!me.descrField && Array.isArray(me.displayTplData))
    {
      return me.displayTplData.map(function (m) { return m[me.descrField] }).join(me.delimiter);
    };
  },

});










/**
 * ЧЕКБОКС
 * @alias Checkbox
 * @manager ZML.InputMgr
 * @registration initComponent
 * @method getValue @property value
 * @method setValue @property value
 */
Ext.define('ZML.input.Checkbox', {

  extend: 'Ext.form.field.Checkbox',
  alias: 'widget.Checkbox',

  mixins: {
    afterEventCallback: 'ZML.mixin.AfterEventCallback',
    formFieldCommons: 'ZML.mixin.FormFieldCommons'
  },

  autoSync: false,



  /** Имя приложения (Ext.application({ name: AppNAME })) */
  AppNAME: 'APP',




  initComponent: function ()
  {
    var me = this;
    this.callParent(arguments);
    me.on({
      validitychange: me.OnValiditychange,
      writeablechange: me.OnWriteablechange,
      scope: me
    });

    GLOBAL[this.AppNAME].Input.register(me);
  },


  listeners: {
    change: function ()
    {
      var me = this;
      me.OnChange();
    }
  },


  OnChange: function ()
  {
    var me = this;
    if (me.autoSync) Ext.StoreManager.LoadLinkedStores({ scopeType: 'Input', scopeId: me.itemId });
    me.OnAfterEvent(me.ApiOnChange, me);
    //setTimeout(function ()
    //{
    //  me.OnAfterEvent(me.ApiOnChange, me);
    //}, 0);
  }
});






/**
 * ПОЛЕ ВВОДА ДАТЫ
 * @alias DateField
 * @manager ZML.InputMgr
 * @registration initComponent
 * @method getValue @property value
 * @method GetDay @property day
 * @method GetMonth @property month
 * @method GetYear @property year
 * @method setValue @property value
 * @method SetDay @property day
 * @method SetMonth @property month
 * @method SetYear @property year
 */
Ext.define('ZML.input.Date', {

  extend: 'Ext.form.field.Date',
  alias: 'widget.DateField',

  mixins: {
    afterEventCallback: 'ZML.mixin.AfterEventCallback',
    formFieldCommons: 'ZML.mixin.FormFieldCommons'
  },

  format: 'd.m.Y',
  submitFormat: 'Y-m-d',
  startDay: 1,
  autoSync: false,



  /** Имя приложения (Ext.application({ name: AppNAME })) */
  AppNAME: 'APP',




  initComponent: function ()
  {
    var me = this;
    this.callParent(arguments);
    me.addEvents('formatChanged', 'dayChanged', 'monthChanged', 'yearChanged');
    me.on({
      formatChanged: me.OnFormatChanged,
      dayChanged: me.OnDayChanged,
      monthChanged: me.OnMonthChanged,
      yearChanged: me.OnYearChanged,
      validitychange: me.OnValiditychange,
      writeablechange: me.OnWriteablechange,
      scope: me
    });

    GLOBAL[this.AppNAME].Input.register(me);
  },


  listeners: {
    change: function ()
    {
      var me = this;
      me.OnChange();
    }
  },


  GetDay: function ()
  {
    var me = this;
    return me.getValue().getDate();
  },


  GetMonth: function ()
  {
    var me = this;
    return me.getValue().getMonth() + 1;
  },


  GetYear: function ()
  {
    var me = this;
    return me.getValue().getFullYear();
  },


  SetDay: function (day)
  {
    if (day !== undefined && day !== null && !isNaN(parseInt(day)))
    {
      var d = parseInt(day);
      if (d >= 1 && d <= 31)
      {
        var me = this;
        var old = me.GetDay();
        var dt = new Date();
        dt.setDate(d);
        dt.setMonth(me.GetMonth() - 1);
        dt.setFullYear(me.GetYear());
        var newvalue = Ext.util.Format.date(dt, me.format);
        me.setRawValue(newvalue);
        me.fireEvent('dayChanged', me, old, d);
      };
    };
  },


  SetMonth: function (month)
  {
    if (month !== undefined && month !== null && !isNaN(parseInt(month)))
    {
      var m = parseInt(month);
      if (m >= 1 && m <= 12)
      {
        var me = this;
        var old = me.GetMonth();
        var dt = new Date();
        dt.setDate(me.GetDay());
        dt.setMonth(m - 1);
        dt.setFullYear(me.GetYear());
        var newvalue = Ext.util.Format.date(dt, me.format);
        me.setRawValue(newvalue);
        me.fireEvent('monthChanged', me, old, m);
      };
    };
  },


  SetYear: function (year)
  {
    if (year !== undefined && year !== null && !isNaN(parseInt(year)))
    {
      var y = parseInt(year);
      if (y >= 1800 && y <= 9999)
      {
        var me = this;
        var old = me.GetYear();
        var dt = new Date();
        dt.setDate(me.GetDay());
        dt.setMonth(me.GetMonth() - 1);
        dt.setFullYear(y);
        var newvalue = Ext.util.Format.date(dt, me.format);
        me.setRawValue(newvalue);
        me.fireEvent('yearChanged', me, old, y);
      };
    };
  },


  OnDayChanged: function (obj, oldvalue, newvalue)
  {
    if (obj)
    {
      if (obj.autoSync) Ext.StoreManager.LoadLinkedStores({ scopeType: 'input', scopeId: obj.itemId });
      obj.OnAfterPropertyChanged();
    };
  },


  OnMonthChanged: function (obj, oldvalue, newvalue)
  {
    if (obj)
    {
      if (obj.autoSync) Ext.StoreManager.LoadLinkedStores({ scopeType: 'input', scopeId: obj.itemId });
      obj.OnAfterPropertyChanged();
    };
  },


  OnYearChanged: function (obj, oldvalue, newvalue)
  {
    if (obj)
    {
      if (obj.autoSync) Ext.StoreManager.LoadLinkedStores({ scopeType: 'input', scopeId: obj.itemId });
      obj.OnAfterPropertyChanged();
    };
  },


  OnLabelChanged: function (obj, oldvalue, newvalue)
  {
    if (obj)
    {
      if (obj.autoSync) Ext.StoreManager.LoadLinkedStores({ scopeType: 'input', scopeId: obj.itemId });
      obj.OnAfterPropertyChanged();
    };
  },


  OnChange: function ()
  {
    var me = this;
    if (me.autoSync) Ext.StoreManager.LoadLinkedStores({ scopeType: 'Input', scopeId: me.itemId });
    me.OnAfterEvent(me.ApiOnChange, me);
    //setTimeout(function ()
    //{
    //  me.OnAfterEvent(me.ApiOnChange, me);
    //}, 0);
  }
});





/**
 * ПОЛЕ ВВОДА ДИАПАЗОНА ДАТ
 * @alias DateSet
 * @manager ZML.InputMgr
 * @registration initComponent
 */
Ext.define('ZML.input.DateSet', {

  extend: 'Ext.form.FieldSet',
  alias: 'widget.DateSet',

  padding: 0,
  margin: '0 0 5 0',
  border: 0,
  msgTarget: 'side',
  layout: 'hbox',
  width: '100%',

  fieldLabel: '',
  labelWidth: 0,


  delimiter: ' .. ',


  value: null,

  valuesList: [],

  FromDate: null,

  ToDate: null,

  MENU: null,

  /** Ограничивать значение ДО в соответствии с текущим значением ОТ */
  checkRange: true,

  /** Включать значения null в композитные значения этого элемента */
  allowNulls: true,

  /** При вычислении композитных значений элемента использовать getSubmitValue() вместо getValue() */
  useSubmitValues: true,



  /** Имя приложения (Ext.application({ name: AppNAME })) */
  AppNAME: 'APP',




  initComponent: function ()
  {
    var me = this;
    me.callParent(arguments);

    me.on('afterrender', function (obj)
    {
      obj.ComposeOwnItems();
      obj.ComposeOwnMenu();
      setTimeout(function () { obj.ComputeFieldsWidths(); }, 10);
    });

    GLOBAL[this.AppNAME].Input.register(me);
  },




  getValue: function ()
  {
    return this.value;
  },


  getValues: function ()
  {
    return this.valuesList;
  },


  setFromValue: function (value)
  {
    this.FromDate.setValue(value);
  },


  setToValue: function (value)
  {
    this.ToDate.setValue(value);
  },


  isBetween: function (value)
  {
    return this.checkRange === true && this.valuesList[0] && this.valuesList[1] && this.valuesList[0] <= value && value <= this.valuesList[1];
  },


  ComposeOwnItems: function ()
  {
    var me = this;

    me.FromDate = Ext.create('ZML.input.Date', {
      ParentSet: me,
      fieldLabel: me.fieldLabel,
      itemId: me.itemId + '-From',
      labelWidth: me.labelWidth,
      submitValue: false,
      emptyText: 'от',
      tooltip: 'Начало интервала'
    });
    me.ToDate = Ext.create('ZML.input.Date', {
      ParentSet: me,
      itemId: me.itemId + '-To',
      margin: '0 0 0 6',
      submitValue: false,
      emptyText: 'до',
      tooltip: 'Окончание интервала'
    });

    me.FromDate.on('change', function (inp)
    {
      if (inp.ParentSet.checkRange === true)
      {
        var val = inp.getValue();
        inp.ParentSet.ToDate.setMinValue(val);
      }
      else
      {
        inp.ParentSet.ToDate.setMinValue(null);
      };
      inp.ParentSet.SetCompositeValues();
      //var otherVal = inp.ParentSet.ToDate.getValue();
      //inp.ParentSet.valuesList = [];
      //inp.ParentSet.valuesList.push(val, otherVal);
      //inp.ParentSet.value = [inp.getSubmitValue(), inp.ParentSet.delimiter, inp.ParentSet.ToDate.getSubmitValue()].join('');
    });
    me.ToDate.on('change', function (inp)
    {
      //var val = inp.getValue();
      //var otherVal = inp.ParentSet.FromDate.getValue();
      inp.ParentSet.SetCompositeValues();
      //inp.ParentSet.valuesList = [];
      //inp.ParentSet.valuesList.push(otherVal, val);
      //inp.ParentSet.value = [inp.ParentSet.FromDate.getSubmitValue(), inp.ParentSet.delimiter, inp.getSubmitValue()].join('');
    });

    var menubtn = Ext.create('ZML.button.Button', {
      itemId: me.itemId + '-InnerMenuBtn',
      call: 'action',
      iconSet: 'mat-icons',
      __icon: 'more_vert',
      __cls: 'opaque',
      color: 'grey',
      tooltip: '',
      needSelection: 'none',
      OnHandlerCall: function (item, evt)
      {
        me.ShowOwnMenu(item, evt);
      }
    });

    me.add(me.FromDate, me.ToDate, menubtn);
  },


  ComposeOwnMenu: function ()
  {
    var me = this;
    me.MENU = new Ext.menu.Menu({
      id: me.itemId + '-InnerMenu',
      animCollapse: true,
      allowOtherMenus: true,
      defaults: { clickHideDelay: 200 },
      HOST: me,
      items: [
        {
          xtype: 'menucheckitem',
          itemId: 'Item1',
          text: 'Ограничивать ДО >= ОТ',
          checked: me.checkRange,
          handler: function (it)
          {
            it.parentMenu.HOST.checkRange = it.checked;
            if (it.checked === false)
            {
              it.parentMenu.HOST.__FromVal = it.parentMenu.HOST.FromDate.getValue();
              it.parentMenu.HOST.FromDate.setValue(null);
              it.parentMenu.HOST.FromDate.setValue(it.parentMenu.HOST.__FromVal);
              it.parentMenu.HOST.ToDate.validate();
            }
            else
            {
              it.parentMenu.HOST.ToDate.setMinValue(it.parentMenu.HOST.FromDate.getValue());
              it.parentMenu.HOST.ToDate.validate();
            };
            setTimeout(function () { it.parentMenu.HOST.SetCompositeValues(); }, 0);
          }
        },
        {
          xtype: 'menucheckitem',
          itemId: 'Item2',
          text: 'Использовать null в значениях',
          checked: me.allowNulls,
          handler: function (it)
          {
            it.parentMenu.HOST.allowNulls = it.checked;
            it.parentMenu.HOST.SetCompositeValues();
          }
        },
        {
          xtype: 'menucheckitem',
          itemId: 'Item3',
          text: 'Использовать submit-значения',
          checked: me.useSubmitValues,
          handler: function (it)
          {
            it.parentMenu.HOST.useSubmitValues = it.checked;
            it.parentMenu.HOST.SetCompositeValues();
          }
        },
      ],
    });
  },


  ShowOwnMenu: function (it, ev)
  {
    this.MENU.showAt(ev.getXY());
    //this.MENU.alignTo(it, 'bl?', [0, -90], false);
  },


  SetCompositeValues: function ()
  {
    var me = this;

    if (me.FromDate && me.ToDate)
    {
      var fromval = me.useSubmitValues === true ? me.FromDate.getSubmitValue() : me.FromDate.getValue();
      var toval = me.useSubmitValues === true ? me.ToDate.getSubmitValue() : me.ToDate.getValue();

      if ((me.allowNulls === true && (fromval == null || toval == null)) || (fromval != null && toval != null))
      {
        if ((me.checkRange === true && (toval >= fromval)) || me.checkRange === false)
        {
          me.value = [fromval, me.delimiter, toval].join('');
          me.valuesList = [];
          me.valuesList.push(fromval, toval);
        };
      };
    };
  },


  ComputeFieldsWidths: function ()
  {
    var me = this;
    var menubtn = me.query('button')[0];

    var meInnerWidth = me.getWidth() - 6 - 6;
    var fromdateWidth = me.FromDate.getWidth();
    var todateWidth = me.ToDate.getWidth();
    var targetTotalWidth = fromdateWidth + todateWidth + 22 - 6 - 6;
    var currentTotalFieldsWidth = fromdateWidth + todateWidth;

    var extraWidth = targetTotalWidth - currentTotalFieldsWidth;

    me.FromDate.setWidth(fromdateWidth + 2 * extraWidth);
    me.ToDate.setWidth(todateWidth + 2 * extraWidth);

  },

});





/**
 * СОСТАВНОЙ ЭЛЕМЕНТ ВВОДА ДАННЫХ ФОРМАТА "КЛЮЧ: ЗНАЧЕНИЕ"
 * @alias KeyValueInput
 * @manager ZML.InputMgr
 * @registration initComponent
 */
Ext.define('ZML.input.KeyValue', {

  extend: 'Ext.form.FieldContainer',
  alias: 'widget.KeyValueInput',


  mixins: {
    field: 'Ext.form.field.Field',
    afterEventCallback: 'ZML.mixin.AfterEventCallback',
    formFieldCommons: 'ZML.mixin.FormFieldCommons'
  },


  layout: 'hbox',
  width: '100%',
  anchor: '100%',
  border: 0,
  bodyBorder: 0,
  autoSync: false,


  /** Поле ввода ключа */
  KeyInput: null,

  /** Поле ввода значения */
  ValueInput: null,

  /** Разрешать пустые значения в поле ввода значения */
  allowBlank: true,

  /** Разделитель ключа и значения, используемый для построения составного текстового значения этого элемента ввода */
  delimiter: ': ',


  /**
   * Тип данных для поля ввода значения (text|number|date|time)
   * @type {'text'|'number'|'date'|'time'}
   */
  valueType: 'text',






  /** Имя приложения (Ext.application({ name: AppNAME })) */
  AppNAME: 'APP',




  /**
   * Сеттер для свойства value этого элемента ввода.
   * @param {any} value Новое значение для свойства value.
   */
  setValue: function (value)
  {
    var me = this;
    var firstvalue;
    var secondvalue;

    if (!Array.isArray(value))
    {
      var temparray = value ? value.toString().split(me.delimiter) : [null, null];
      firstvalue = temparray[0];
      secondvalue = temparray[1];
    }
    else
    {
      firstvalue = value[0];
      secondvalue = value.length > 1 ? value[1] : null;
    };

    me.KeyInput.setValue(firstvalue);
    me.ValueInput.setValue(secondvalue);

  },


  getValue: function ()
  {
    var cmbvalue = this.KeyInput.getSubmitValue();
    var inpvalue = this.ValueInput.getSubmitValue();
    var value;

    if (cmbvalue && inpvalue)
    {
      value = cmbvalue + this.delimiter + inpvalue;
    }
    else
    {
      value = null;
    };

    return value;
  },


  getSubmitValue: function ()
  {
    return this.getValue();
  },






  listeners: {
    change: function ()
    {
      var me = this;
      var cmbvalue = me.KeyInput.getSubmitValue();
      var inpvalue = me.ValueInput.getSubmitValue();

      if (cmbvalue && (inpvalue || !!me.allowBlank))
      {
        me.value = cmbvalue + me.delimiter + inpvalue;
      }
      else
      {
        me.value = null;
      };

      me.OnChange();
    }
  },





  initComponent: function ()
  {
    var me = this;

    var valInputClass;
    switch (me.valueType)
    {
      case 'text': valInputClass = 'ZML.input.Text'; break;
      case 'number': valInputClass = 'ZML.input.Number'; break;
      case 'date': valInputClass = 'ZML.input.Date'; break;
      case 'time': valInputClass = 'ZML.input.Time'; break;

      default: valInputClass = 'ZML.input.Text'; break;
    };


    me.KeyInput = Ext.create('ZML.input.Text', {
      flex: 1,
      allowBlank: false,
      emptyText: 'key',
      submitEmptyText: false
    });

    me.ValueInput = Ext.create(valInputClass, {
      flex: 1,
      margin: '0 0 0 5',
      allowBlank: me.allowBlank,
      emptyText: 'value (' + me.valueType + ')',
      submitEmptyText: false
    });

    me.KeyInput.on('afterrender', function (obj) { obj.applyEmptyText(); });
    me.ValueInput.on('afterrender', function (obj) { obj.applyEmptyText(); });

    me.items = [me.KeyInput, me.ValueInput];
    me.callParent(arguments);

    me.relayEvents(me.KeyInput, ['change']);
    me.relayEvents(me.ValueInput, ['change']);

    me.initField();

    GLOBAL[this.AppNAME].Input.register(me);
  },







  OnChange: function ()
  {
    var me = this;
    if (me.autoSync) Ext.StoreManager.LoadLinkedStores({ scopeType: 'Input', scopeId: me.itemId });
    me.OnAfterEvent(me.ApiOnChange, me);
  },






  /**
   * Возвращает значение "ключа".
   * @method
   */
  GetKeyPart: function ()
  {
    return this.KeyInput.getSubmitValue();
  },

  /**
   * Возвращает значение "значения".
   * @method
   */
  GetValuePart: function ()
  {
    return this.ValueInput.getValue();
  },

  /**
   * Возвращает составное значение в формате {ключ}{разделитель}{значение}
   * @method
   */
  GetFullValue: function ()
  {
    return this.getValue();
  },

  /**
   * Устанавливает значение "ключа".
   * @method
   * @param {any} val Новое значение "ключа".
   */
  SetKeyPart: function (val)
  {
    this.KeyInput.setValue(val);
  },

  /**
   * Устанавливает значение "значения".
   * @method
   * @param {any} val Новое значение "значения".
   */
  SetValuePart: function (val)
  {
    this.ValueInput.setValue(val);
  },

  /**
   * Устанавливает строку разделителя.
   * @method
   * @param {any} val Новая строка разделителя.
   */
  SetDelimiter: function (val)
  {
    this.delimiter = val;
  },

});





/**
 * ПОЛЕ ВВОДА МЕСЯЦА
 * @alias MonthField
 * @manager ZML.ComboMgr
 * @registration initComponent
 */
Ext.define('ZML.input.Month', {

  extend: 'ZML.input.ComboBox',
  alias: 'widget.MonthField',


  queryMode: 'local',

  valueField: 'Id',
  displayField: 'Name',
  descrField: 'ShortName',



  /** Имя приложения (Ext.application({ name: AppNAME })) */
  AppNAME: 'APP',



  TEMPLATE: [],





  initComponent: function ()
  {
    var me = this;
    me.callParent(arguments);
    me.ComposeStore();

    me.on({
      afterrender: me.OnAfterRender,
      change: me.OnChange,
      scope: me
    });

    me.inputElem = 'input';

    GLOBAL[this.AppNAME].Combo.register(this);
  },







  OnAfterRender: function (obj)
  {
    if (obj)
    {
      if (Array.isArray(obj.TEMPLATE) && obj.TEMPLATE.length)
      {
        obj.tpl = new Ext.XTemplate(obj.TEMPLATE);
      };

      setTimeout(function ()
      {
        obj.CountFoundItems();
        obj.SelectRecordByValue();
      }, 500);
    };
  },


  OnChange: function (obj)
  {
    if (obj)
    {
      obj.updateClearIcon();
      obj.CountFoundItems();
      Ext.StoreManager.LoadLinkedStores({ scopeType: 'Input', scopeId: obj.itemId })
      obj.OnAfterEvent(obj.ApiOnChange, obj);
      //setTimeout(function ()
      //{
      //  obj.OnAfterEvent(obj.ApiOnChange, obj);
      //}, 0);
    };
  },


  ComposeStore: function ()
  {
    var me = this;
    var modelname = me.itemId + '-MonthsModel';

    Ext.define(modelname, {
      extend: 'Ext.data.Model',
      idProperty: me.valueField,
      fields: [
        { name: me.valueField, type: 'int' },
        { name: me.displayField, type: 'string' },
        { name: me.descrField, type: 'string' },
      ]
    });

    var stor = Ext.create('Ext.data.Store', {
      model: modelname,
      MonthCombo: me,
      data: [].concat(me.GetMonthsList(modelname))
    });

    me.bindStore(stor);
  },


  GetMonthsList: function (modelname)
  {
    var me = this;
    var array = [
      [1, 'январь', 'янв'],
      [2, 'февраль', 'фев'],
      [3, 'март', 'мар'],
      [4, 'апрель', 'апр'],
      [5, 'май', 'май'],
      [6, 'июнь', 'июн'],
      [7, 'июль', 'июл'],
      [8, 'август', 'авг'],
      [9, 'сентябрь', 'сен'],
      [10, 'октябрь', 'окт'],
      [11, 'ноябрь', 'ноя'],
      [12, 'декабрь', 'дек'],
    ];

    var ret = [];

    array.forEach(function (f)
    {
      var obj = {};
      obj[me.valueField] = f[0];
      obj[me.displayField] = f[1];
      obj[me.descrField] = f[2];
      var record = Ext.create(modelname, obj);
      ret.push(record);
    });

    return ret;
  },


  SelectRecordByValue: function ()
  {
    var me = this;
    if (me.store && me.store.isStore)
    {
      var record = me.store.findRecord(me.valueField, me.defaultValue);
      me.select(record);
    };
  },


  CountFoundItems: function ()
  {
    var emptytext = '' + this.store.getCount() + ' элем.';
    this.emptyText = emptytext;
    this.applyEmptyText();
  },

});




/**
 * ПОЛЕ ВВОДА ЧИСЕЛ
 * @alias NumberField
 * @manager ZML.InputMgr
 * @registration initComponent
 * 
 * @method getValue @property value
 * @method setValue @property value
 * @method GetStep @property step
 * @method SetStep @property step
 * @method GetMinValue @property minValue
 * @method setMinValue @property minValue
 * @method GetMaxValue @property maxValue
 * @method setMaxValue @property maxValue
 */
Ext.define('ZML.input.Number', {

  extend: 'Ext.form.field.Number',
  alias: 'widget.NumberField',

  mixins: {
    afterEventCallback: 'ZML.mixin.AfterEventCallback',
    formFieldCommons: 'ZML.mixin.FormFieldCommons'
  },

  validateOnChange: true,
  autoSync: false,



  /** Имя приложения (Ext.application({ name: AppNAME })) */
  AppNAME: 'APP',



  initComponent: function ()
  {
    var me = this;
    this.callParent(arguments);
    me.addEvents('formatChanged');
    me.on({
      formatChanged: me.OnFormatChanged,
      validitychange: me.OnValiditychange,
      writeablechange: me.OnWriteablechange,
      scope: me
    });

    GLOBAL[this.AppNAME].Input.register(me);
  },


  listeners: {
    change: function (obj, newvalue, oldvalue)
    {
      var me = this;
      if (me.minValue !== undefined && me.minValue !== null && newvalue < me.minValue)
      {
        me.setValue(me.minValue);
      };
      if (me.maxValue !== undefined && me.maxValue !== null && newvalue > me.maxValue)
      {
        me.setValue(me.maxValue);
      };
      me.OnChange();
    }
  },


  clearField: function ()
  {
    this.setValue(null);
  },





  GetMinValue: function ()
  {
    return this.minValue;
  },


  GetMaxValue: function ()
  {
    return this.maxValue;
  },


  GetStep: function ()
  {
    return this.step;
  },


  SetStep: function (value)
  {
    this.step = value;
  },


  OnAfterPropertyChanged: function ()
  {
    var me = this;
    if (me.isValid())
    {
      if (me.autoSync) Ext.StoreManager.LoadLinkedStores({ scopeType: 'Input', scopeId: me.itemId });
    };
  },


  OnChange: function ()
  {
    var me = this;
    if (me.autoSync) Ext.StoreManager.LoadLinkedStores({ scopeType: 'Input', scopeId: me.itemId });
    me.OnAfterEvent(me.ApiOnChange, me);
    //setTimeout(function ()
    //{
    //  me.OnAfterEvent(me.ApiOnChange, me);
    //}, 0);
  }

});
























/**
 * ПОЛЕ ВВОДА ДИАПАЗОНА ЧИСЕЛ
 * @alias NumberSet
 * @manager ZML.InputMgr
 * @registration initComponent
 */
Ext.define('ZML.input.NumberSet', {

  extend: 'Ext.form.FieldSet',
  alias: 'widget.NumberSet',

  padding: 0,
  margin: '0 0 5 0',
  border: 0,
  msgTarget: 'side',
  layout: 'hbox',
  width: '100%',

  fieldLabel: '',
  labelWidth: 0,
  step: 1,
  minValue: null,
  maxValue: null,


  delimiter: ' .. ',


  value: null,

  valuesList: [],

  /** Поле для ввода начального значения интервала */
  FromNum: null,

  /** Поле для ввода конечного значения интервала */
  ToNum: null,

  MENU: null,

  /** Ограничивать значение ДО в соответствии с текущим значением ОТ */
  checkRange: true,

  /** Включать значения null в композитные значения этого элемента */
  allowNulls: true,

  /** При вычислении композитных значений элемента использовать getSubmitValue() вместо getValue() */
  useSubmitValues: true,



  /** Имя приложения (Ext.application({ name: AppNAME })) */
  AppNAME: 'APP',




  initComponent: function ()
  {
    var me = this;
    me.callParent(arguments);

    me.on('afterrender', function (obj)
    {
      obj.ComposeOwnItems();
      obj.ComposeOwnMenu();
      setTimeout(function () { obj.ComputeFieldsWidths(); }, 10);
    });

    GLOBAL[this.AppNAME].Input.register(me);
  },




  getValue: function ()
  {
    return this.value;
  },


  getValues: function ()
  {
    return this.valuesList;
  },


  setFromValue: function (value)
  {
    this.FromNum.setValue(value);
  },


  setToValue: function (value)
  {
    this.ToNum.setValue(value);
  },


  isBetween: function (value)
  {
    return this.checkRange === true && this.valuesList[0] && this.valuesList[1] && this.valuesList[0] <= value && value <= this.valuesList[1];
  },


  ComposeOwnItems: function ()
  {
    var me = this;

    me.FromNum = Ext.create('ZML.input.Number', {
      ParentSet: me,
      fieldLabel: me.fieldLabel,
      itemId: me.itemId + '-From',
      labelWidth: me.labelWidth,
      submitValue: false,
      emptyText: 'от',
      tooltip: 'Начало интервала',
      step: me.step,
      minValue: me.minValue
    });
    me.ToNum = Ext.create('ZML.input.Number', {
      ParentSet: me,
      itemId: me.itemId + '-To',
      margin: '0 0 0 6',
      submitValue: false,
      emptyText: 'до',
      tooltip: 'Окончание интервала',
      step: me.step,
      maxValue: me.maxValue
    });

    me.FromNum.on('change', function (inp)
    {
      var parent = inp.ParentSet;

      if (parent.checkRange === true)
      {
        var val = inp.getValue();
        parent.ToNum.setMinValue(val);
      }
      else
      {
        parent.ToNum.setMinValue(null);
      };
      parent.SetCompositeValues();
    });
    me.ToNum.on('change', function (inp)
    {
      inp.ParentSet.SetCompositeValues();
    });

    var menubtn = Ext.create('ZML.button.Button', {
      itemId: me.itemId + '-InnerMenuBtn',
      call: 'action',
      iconSet: 'mat-icons',
      __icon: 'more_vert',
      __cls: 'opaque',
      color: 'grey',
      tooltip: '',
      needSelection: 'none',
      OnHandlerCall: function (item, evt)
      {
        me.ShowOwnMenu(item, evt);
      }
    });

    me.add(me.FromNum, me.ToNum, menubtn);
  },


  ComposeOwnMenu: function ()
  {
    var me = this;
    me.MENU = new Ext.menu.Menu({
      id: me.itemId + '-InnerMenu',
      animCollapse: true,
      allowOtherMenus: true,
      defaults: { clickHideDelay: 200 },
      HOST: me,
      items: [
        {
          xtype: 'menucheckitem',
          itemId: 'Item1',
          text: 'Ограничивать ДО >= ОТ',
          checked: me.checkRange,
          handler: function (it)
          {
            var host = it.parentMenu.HOST;
            host.checkRange = it.checked;
            if (it.checked === false)
            {
              host.__FromVal = host.FromNum.getValue();
              host.FromNum.setValue(null);
              host.FromNum.setValue(host.__FromVal);
              host.ToNum.validate();
            }
            else
            {
              host.ToNum.setMinValue(host.FromNum.getValue());
              host.ToNum.validate();
            };
            setTimeout(function () { host.SetCompositeValues(); }, 0);
          }
        },
        {
          xtype: 'menucheckitem',
          itemId: 'Item2',
          text: 'Использовать null в значениях',
          checked: me.allowNulls,
          handler: function (it)
          {
            it.parentMenu.HOST.allowNulls = it.checked;
            it.parentMenu.HOST.SetCompositeValues();
          }
        },
        {
          xtype: 'menucheckitem',
          itemId: 'Item3',
          text: 'Использовать submit-значения',
          checked: me.useSubmitValues,
          handler: function (it)
          {
            it.parentMenu.HOST.useSubmitValues = it.checked;
            it.parentMenu.HOST.SetCompositeValues();
          }
        },
      ],
    });
  },


  ShowOwnMenu: function (it, ev)
  {
    this.MENU.showAt(ev.getXY());
    //this.MENU.alignTo(it, 'bl?', [0, -90], false);
  },


  SetCompositeValues: function ()
  {
    var me = this;

    if (me.FromNum && me.ToNum)
    {
      var fromval = me.useSubmitValues === true ? me.FromNum.getSubmitValue() : me.FromNum.getValue();
      var toval = me.useSubmitValues === true ? me.ToNum.getSubmitValue() : me.ToNum.getValue();

      if ((me.allowNulls === true && (fromval == null || toval == null)) || (fromval != null && toval != null))
      {
        if ((me.checkRange === true && (toval >= fromval)) || me.checkRange === false)
        {
          me.value = [fromval, me.delimiter, toval].join('');
          me.valuesList = [];
          me.valuesList.push(fromval, toval);
        };
      };
    };
  },


  ComputeFieldsWidths: function ()
  {
    var me = this;
    var menubtn = me.query('button')[0];

    var meInnerWidth = me.getWidth() - 6 - 6;
    var fromWidth = me.FromNum.getWidth();
    var toWidth = me.ToNum.getWidth();
    var targetTotalWidth = fromWidth + toWidth + 22 - 6 - 6;
    var currentTotalFieldsWidth = fromWidth + toWidth;

    var extraWidth = targetTotalWidth - currentTotalFieldsWidth;

    me.FromNum.setWidth(fromWidth + 2 * extraWidth);
    me.ToNum.setWidth(toWidth + 2 * extraWidth);

  },

});





/**
 * ПОЛЕ ВВОДА ТЕКСТА
 * @alias TextField
 * @manager ZML.InputMgr
 * @registration initComponent
 * @method getValue @property value
 * @method setValue @property value
 */
Ext.define('ZML.input.Text', {

  extend: 'Ext.form.field.Text',
  alias: 'widget.TextField',

  mixins: {
    afterEventCallback: 'ZML.mixin.AfterEventCallback',
    processQuotes: 'ZML.mixin.ProcessQuotes',
    formFieldCommons: 'ZML.mixin.FormFieldCommons',
    handleClearIcon: 'ZML.mixin.HandleClearIcon'
  },


  autoSync: false,



  /** Имя приложения (Ext.application({ name: AppNAME })) */
  AppNAME: 'APP',




  initComponent: function ()
  {
    var me = this;
    this.callParent(arguments);
    me.addEvents('formatChanged');
    me.on({
      formatChanged: me.OnFormatChanged,
      validitychange: me.OnValiditychange,
      writeablechange: me.OnWriteablechange,
      scope: me
    });
    me.inputElem = 'input';

    GLOBAL[this.AppNAME].Input.register(me);
  },

  listeners: {
    change: function ()
    {
      var me = this;
      if (me.convertQuotes === true && me.CountQuotes() > 0)
      {
        me.ReplaceQuotes();
      };
      me.OnChange();
    }
  },


  OnLabelChanged: function (obj, oldvalue, newvalue)
  {
    if (obj)
    {
      obj.scope.OnAfterPropertyChanged();
    };
  },


  OnChange: function ()
  {
    var me = this;
    me.updateClearIcon();
    if (me.autoSync) Ext.StoreManager.LoadLinkedStores({ scopeType: 'Input', scopeId: me.itemId });
    me.OnAfterEvent(me.ApiOnChange, me);
    //setTimeout(function ()
    //{
    //  me.OnAfterEvent(me.ApiOnChange, me);
    //}, 0);
  }
});





/**
 * МНОГОСТРОЧНОЕ ПОЛЕ ВВОДА ТЕКСТА
 * @alias TextArea
 * @manager ZML.InputMgr
 * @registration initComponent
 * @method getValue @property value
 * @method setValue @property value
 */
Ext.define('ZML.input.TextArea', {

  extend: 'Ext.form.field.TextArea',
  alias: 'widget.TextArea',

  mixins: {
    afterEventCallback: 'ZML.mixin.AfterEventCallback',
    processQuotes: 'ZML.mixin.ProcessQuotes',
    formFieldCommons: 'ZML.mixin.FormFieldCommons',
    handleClearIcon: 'ZML.mixin.HandleClearIcon'
  },

  autoSync: false,



  /** Имя приложения (Ext.application({ name: AppNAME })) */
  AppNAME: 'APP',




  initComponent: function ()
  {
    var me = this;
    this.callParent(arguments);
    me.addEvents('formatChanged');
    me.on({
      formatChanged: me.OnFormatChanged,
      validitychange: me.OnValiditychange,
      writeablechange: me.OnWriteablechange,
      scope: me
    });
    me.inputElem = 'textarea';

    GLOBAL[this.AppNAME].Input.register(me);
  },


  listeners: {
    change: function ()
    {
      var me = this;
      if (me.convertQuotes === true && me.CountQuotes() > 0)
      {
        me.ReplaceQuotes();
      };
      me.OnChange();
    }
  },


  OnLabelChanged: function (obj, oldvalue, newvalue)
  {
    if (obj)
    {
      obj.scope.OnAfterPropertyChanged();
    };
  },


  OnChange: function ()
  {
    var me = this;
    me.updateClearIcon();
    if (me.autoSync) Ext.StoreManager.LoadLinkedStores({ scopeType: 'Input', scopeId: me.itemId });
    me.OnAfterEvent(me.ApiOnChange, me);
    //setTimeout(function ()
    //{
    //  me.OnAfterEvent(me.ApiOnChange, me);
    //}, 0);
  }
});





/**
 * ПОЛЕ ВВОДА ВРЕМЕНИ
 * @alias TimeField
 * @manager ZML.InputMgr
 * @registration initComponent
 * @method getValue @property value
 * @method GetHours @property hours
 * @method GetMinutes @property minutes
 * @method GetSeconds @property seconds
 * @method setValue @property value
 * @method SetHours @property hours
 * @method SetMinutes @property minutes
 * @method SetSeconds @property seconds
 */
Ext.define('ZML.input.Time', {

  extend: 'Ext.form.field.Time',
  alias: 'widget.TimeField',

  mixins: {
    afterEventCallback: 'ZML.mixin.AfterEventCallback',
    formFieldCommons: 'ZML.mixin.FormFieldCommons'
  },

  format: 'H:i',
  submitFormat: 'H:i',
  autoSync: false,



  /** Имя приложения (Ext.application({ name: AppNAME })) */
  AppNAME: 'APP',




  initComponent: function ()
  {
    var me = this;
    this.callParent(arguments);
    me.addEvents('formatChanged', 'hoursChanged', 'minutesChanged', 'secondsChanged');
    me.on({
      formatChanged: me.OnFormatChanged,
      hoursChanged: me.OnHoursChanged,
      minutesChanged: me.OnMinutesChanged,
      secondsChanged: me.OnSecondsChanged,
      validitychange: me.OnValiditychange,
      writeablechange: me.OnWriteablechange,
      scope: me
    });

    GLOBAL[this.AppNAME].Input.register(me);
  },


  listeners: {
    change: function ()
    {
      var me = this;
      me.OnChange();
    }
  },


  GetHours: function ()
  {
    var me = this;
    return me.getValue().getHours();
  },


  GetMinutes: function ()
  {
    var me = this;
    return me.getValue().getMinutes();
  },


  GetSeconds: function ()
  {
    var me = this;
    return me.getValue().getSeconds();
  },


  SetHours: function (hours)
  {
    if (hours !== undefined && hours !== null && !isNaN(parseInt(hours)))
    {
      var hrs = parseInt(hours);
      if (hrs >= 0 && hrs <= 23)
      {
        var me = this;
        var old = me.GetHours();
        var dt = new Date();
        dt.setHours(hrs);
        dt.setMinutes(me.GetMinutes());
        dt.setSeconds(me.GetSeconds());
        var newvalue = Ext.util.Format.date(dt, me.format);
        me.setRawValue(newvalue);
        me.fireEvent('hoursChanged', me, old, hrs);
      };
    };
  },


  SetMinutes: function (minutes)
  {
    if (minutes !== undefined && minutes !== null && !isNaN(parseInt(minutes)))
    {
      var mins = parseInt(minutes);
      if (mins >= 0 && mins <= 59)
      {
        var me = this;
        var old = me.GetMinutes();
        var dt = new Date();
        dt.setHours(me.GetHours());
        dt.setMinutes(mins);
        dt.setSeconds(me.GetSeconds());
        var newvalue = Ext.util.Format.date(dt, me.format);
        me.setRawValue(newvalue);
        me.fireEvent('minutesChanged', me, old, mins);
      };
    };
  },


  SetSeconds: function (seconds)
  {
    if (seconds !== undefined && seconds !== null && !isNaN(parseInt(seconds)))
    {
      var secs = parseInt(seconds);
      if (secs >= 0 && secs <= 59)
      {
        var me = this;
        var old = me.GetSeconds();
        var dt = new Date();
        dt.setHours(me.GetHours());
        dt.setMinutes(me.GetMinutes());
        dt.setSeconds(secs);
        var newvalue = Ext.util.Format.date(dt, me.format);
        me.setRawValue(newvalue);
        me.fireEvent('secondsChanged', me, old, secs);
      };
    };
  },


  OnHoursChanged: function (obj, oldvalue, newvalue)
  {
    if (obj)
    {
      if (obj.autoSync) Ext.StoreManager.LoadLinkedStores({ scopeType: 'input', scopeId: obj.itemId });
      obj.OnAfterPropertyChanged();
    };
  },


  OnMinutesChanged: function (obj, oldvalue, newvalue)
  {
    if (obj)
    {
      if (obj.autoSync) Ext.StoreManager.LoadLinkedStores({ scopeType: 'input', scopeId: obj.itemId });
      obj.OnAfterPropertyChanged();
    };
  },


  OnSecondsChanged: function (obj, oldvalue, newvalue)
  {
    if (obj)
    {
      if (obj.autoSync) Ext.StoreManager.LoadLinkedStores({ scopeType: 'input', scopeId: obj.itemId });
      obj.OnAfterPropertyChanged();
    };
  },


  OnLabelChanged: function (obj, oldvalue, newvalue)
  {
    if (obj)
    {
      if (obj.autoSync) Ext.StoreManager.LoadLinkedStores({ scopeType: 'input', scopeId: obj.itemId });
      obj.OnAfterPropertyChanged();
    };
  },


  OnChange: function ()
  {
    var me = this;
    if (me.autoSync) Ext.StoreManager.LoadLinkedStores({ scopeType: 'Input', scopeId: me.itemId });
    me.OnAfterEvent(me.ApiOnChange, me);
    //setTimeout(function ()
    //{
    //  me.OnAfterEvent(me.ApiOnChange, me);
    //}, 0);
  }
});















/**
 * ЭЛЕМЕНТ ДЛЯ РЕДАКТИРОВАНИЯ СПИСКА ЗНАЧЕНИЙ
 * @alias Collection
 * @manager ZML.InputMgr
 * @registration initComponent
 * 
 */
Ext.define('ZML.input.Collection', {

  extend: 'Ext.form.FieldSet',
  alias: 'widget.Collection',


  mixins: {
    afterEventCallback: 'ZML.mixin.AfterEventCallback',
    formFieldCommons: 'ZML.mixin.FormFieldCommons'
  },


  layout: 'vbox',
  width: '100%',
  height: '100%',
  flex: 1,
  overflowY: 'auto',
  border: 0,
  padding: 5,


  /**
   * Тип элементов списка.
   * @type {'text'|'number'|'date'|'keyvalue'}
   */
  itemsType: 'text',

  /** Разделитель элементов списка (для текстового представления списка) */
  itemsDelimiter: '; ',

  /** Разделитель для элементов списка типа "ключ: значение" */
  kvDelimiter: ': ',

  /** Отображать индексы элементов списка */
  showIndices: false,

  uniqueItems: true,
  allowEmpty: false,
  autoSync: false,
  useSubmitValues: true,


  /**
   * Представление данных, содержащихся в коллекции.
   * @type {'string'|'array'|'object'}
   */
  submitAs: 'array',


  value: null,






  AppNAME: 'APP',






  listeners: {
    afterrender: function ()
    {
      var me = this;
      me.REORDERER = Ext.create('Ext.ux.BoxReorderer', {
        animate: false,
        itemSelector: '.logx-expr-group',
        listeners: {
          Drop: function (plug, obj, item, start, end)
          {
            obj.OnItemDropped(plug, obj, item, start, end);
          },
        }
      });
      me.addPlugin(me.REORDERER);
      me.REORDERER.afterFirstLayout();

      me.AddNewElement();
    },
    change: function ()
    {
      this.SetSelfValue();
      this.OnChange();
    }
  },






  initComponent: function ()
  {
    var me = this;
    me.callParent(arguments);

    GLOBAL[this.AppNAME].Input.register(me);
  },





  getValue: function ()
  {
    return this.value;
  },

  getSubmitValue: function ()
  {
    return this.getValue();
  },




  GetElementClass: function ()
  {
    switch (this.itemsType)
    {
      case 'text': return 'ZML.input.Text';
      case 'number': return 'ZML.input.Number';
      case 'date': return 'ZML.input.Date';
      case 'keyvalue': return 'ZML.input.KeyValue';

      default: return 'ZML.input.Text';
    };
  },


  CreateNewElement: function ()
  {
    var me = this;
    var elemClass = me.GetElementClass();
    var ret = Ext.create(elemClass, {
      Parent: me,
      fieldLabel: me.fieldLabel,
      itemId: me.itemId + '-Editor',
      labelWidth: me.labelWidth,
      columnWidth: 1,
      margin: '0 6 0 0',
      submitValue: false,
      delimiter: me.kvDelimiter,
    });
    return ret;
  },


  CreateContainer: function ()
  {
    var me = this;
    var elem = me.CreateNewElement();
    var cont = Ext.create('ZML.input.Wrapper', {
      HOST: me,
      INPUT: elem,
    });

    return cont;
  },


  AddNewElement: function (afterIndex)
  {
    var me = this;

    if (+afterIndex < 0)
    {
      afterIndex = 0;
    };
    if (+afterIndex > me.GetElementsCount())
    {
      afterIndex = me.GetElementsCount();
    };

    var item = me.CreateContainer();
    me.insert(afterIndex + 1, item);
    me.updateLayout();

    me.relayEvents(item.INPUT, ['change']);

    me.RefreshElementDelButtons();

    me.CalculateIndices();
  },


  DeleteElement: function (element)
  {
    this.remove(element);
    this.RefreshElementDelButtons();
    this.CalculateIndices();
  },


  DeleteElementByIndex: function (index)
  {
    var element = this.GetElementByIndex(index);
    if (!!element)
    {
      this.DeleteElement(element);
    };
  },







  CalculateIndices: function ()
  {
    var me = this;
    me.query('InputWrapper').forEach(function (f, i)
    {
      f.setIndex(i);
    });
  },


  RefreshElementDelButtons: function ()
  {
    var amount = this.GetElementsCount();
    if (amount < 2)
    {
      if (!!this.GetElementByIndex(0).DelBtn) this.GetElementByIndex(0).DelBtn.setDisabled(true);
    }
    else
    {
      this.query('InputWrapper').filter(function (f) { return f.DelBtn.disabled === true }).forEach(function (el) { el.DelBtn.setDisabled(false) });
    };
  },


  GetElementValues: function ()
  {
    var me = this;
    var temp = this.query('InputWrapper').map(function (m) { return m.INPUT });
    switch (me.submitAs)
    {
      case 'string':
        return temp.map(function (mm) { return me.useSubmitValues ? mm.getSubmitValue() : mm.getValue() }).filter(function (f) { return !!f }).join(me.itemsDelimiter);


      case 'array':
        if (me.itemsType === 'keyvalue')
        {
          return temp.filter(function (f) { return !!f.GetKeyPart() }).map(function (mm) { var obj = {}; obj[mm.GetKeyPart()] = mm.GetValuePart(); return obj; });
        }
        else
        {
          return temp.map(function (mm) { return me.useSubmitValues ? mm.getSubmitValue() : mm.getValue() }).filter(function (f) { return !!f });
        };


      case 'object':
        if (me.itemsType === 'keyvalue')
        {
          var obj = {};
          temp.forEach(function (f)
          {
            var tempObj = {};
            var key = f.GetKeyPart();
            if (!!key)
            {
              tempObj[key] = f.GetValuePart();
            };
            Ext.apply(obj, tempObj);
          });
          return obj;
        }
        else
        {
          return temp.map(function (mm) { return me.useSubmitValues ? mm.getSubmitValue() : mm.getValue() });
        };



      default: return temp.map(function (mm) { return me.useSubmitValues ? mm.getSubmitValue() : mm.getValue() });
    };
  },


  GetElementsCount: function ()
  {
    return this.query('InputWrapper').length;
  },


  GetElementByIndex: function (index)
  {
    if (index > -1 && index < this.GetElementsCount())
    {
      return this.query('InputWrapper')[index];
    }
    else
    {
      return null;
    }
  },


  SetSelfValue: function ()
  {
    this.value = this.GetElementValues();
  },







  OnChange: function ()
  {
    var me = this;
    if (me.autoSync) Ext.StoreManager.LoadLinkedStores({ scopeType: 'Input', scopeId: me.itemId });
    me.OnAfterEvent(me.ApiOnChange, me);
  },


  OnItemDropped: function (plug, obj, item, start, end)
  {
    setTimeout(() => obj.CalculateIndices(), 100);
  },

});


/**
 * ВСПОМОГАТЕЛЬНЫЙ КЛАСС
 * КОНТЕЙНЕР ДЛЯ ДОЧЕРНИХ ЭЛЕМЕНТОВ РЕДАКТОРА СПИСКОВ
 */
Ext.define('ZML.input.Wrapper', {

  extend: 'Ext.container.Container',
  alias: 'widget.InputWrapper',


  padding: '0 6',
  margin: '2 0',
  cls: 'logx-expr-group',
  layout: 'column',
  width: '100%',


  config: {
    index: null,
  },



  HOST: null,

  INPUT: null,

  INDEX: null,

  AddBtn: null,

  DelBtn: null,






  setIndex: function (value)
  {
    var me = this;
    var oldvalue = me.getIndex();
    me.index = value;
    if (oldvalue != value)
    {
      me.fireEvent('indexChanged', me, oldvalue, value);
    };
  },






  initComponent: function (config)
  {
    var me = this;
    config = config || {};
    me.initConfig(config);
    me.callParent(arguments);
    me.addEvents('indexChanged');

    me.on({
      indexChanged: me.OnIndexChanged,
      afterrender: me.OnAfterRender,
      scope: me
    });
  },





  ComposeIndexElement: function ()
  {
    var me = this;
    me.INDEX = me.INDEX || Ext.create('Ext.form.field.Display', {
      itemId: me.itemId + '-index',
      cls: 'logx-align-start',
      width: 24,
      height: 22,
      value: me.getIndex()
    });
  },


  ComposeOwnButtons: function ()
  {
    var me = this;
    var totalElements = me.HOST.GetElementsCount();
    me.AddBtn = me.AddBtn || Ext.create('Ext.button.Button', {
      Parent: me,
      itemId: me.itemId + '-action-add',
      cls: 'logx-icon-btn opaque mat-icons',
      iconCls: 'add darkblue',
      width: 22,
      height: '100%',
      tooltip: 'Добавить элемент',
      handler: function ()
      {
        var parent = this.Parent;
        parent.HOST.AddNewElement(parent.getIndex());
      },
    });
    me.DelBtn = me.DelBtn || Ext.create('Ext.button.Button', {
      Parent: me,
      itemId: me.itemId + '-action-delete',
      cls: 'logx-icon-btn opaque mat-icons',
      iconCls: 'remove darkred',
      disabled: totalElements < 2,
      width: 22,
      height: '100%',
      tooltip: 'Удалить элемент',
      handler: function ()
      {
        var parent = this.Parent;
        parent.HOST.DeleteElementByIndex(parent.getIndex());
      },
    });
  },






  OnAfterRender: function ()
  {
    var me = this;
    me.ComposeIndexElement();
    me.ComposeOwnButtons();
    if (!!me.HOST.showIndices)
    {
      me.add([me.INDEX, me.INPUT, me.DelBtn, me.AddBtn]);
    }
    else
    {
      me.add([me.INPUT, me.DelBtn, me.AddBtn]);
    };
  },


  OnIndexChanged: function (obj, oldval, newval)
  {
    if (obj && obj.INDEX)
    {
      obj.INDEX.setValue(newval);
    };
  },

});












/**
 * ПАНЕЛЬ
 * @alias Panel
 * @manager ZML.PanelMgr
 * @registration initComponent
 */
Ext.define('ZML.panel.Panel', {

  extend: 'Ext.panel.Panel',
  alias: 'widget.Panel',


  mixins: {
    loadable: 'ZML.mixin.Loadable',
  },


  layout: 'fit',
  height: '100%',
  width: '100%',
  flex: 1,


  /** Имя приложения (Ext.application({ name: AppNAME })) */
  AppNAME: 'APP',



  listeners: {
    afterrender: function (obj)
    {
      obj.InitLoader();
    }
  },




  initComponent: function (config)
  {
    var me = this;
    me.callParent(arguments);

    me.initConfig(config);

    GLOBAL[this.AppNAME].Panel.register(me);
  },



  GetCollapsed: function ()
  {
    var me = this;
    return !!me.collapsed;
  },

  SetCollapsed: function (value)
  {
    var me = this;
    if (!!value)
    {
      me.collapse();
    }
    else
    {
      me.expand();
    };
  },





  /**
   * Заменяет содержимое панели на указанную HTML-разметку.
   * @param {String} content HTML-строка.
   */
  AddHtmlContent: function (content)
  {
    //this.body.el.dom.innerHTML = content;
    this.update(content, true);
  },

  /**
   * Очищает HTML-содержимое панели.
   */
  RemoveHtmlContent: function ()
  {
    this.body.el.dom.innerHTML = '';
  },


});










Ext.define('ZML.panel.Tabpanel', {

  extend: 'Ext.tab.Panel',
  alias: 'widget.Tabpanel',


  plain: false,
  height: '100%',
  width: '100%',
  flex: 1,


  /** Имя приложения (Ext.application({ name: AppNAME })) */
  AppNAME: 'APP',





  initComponent: function (config)
  {
    var me = this;
    me.callParent(arguments);

    me.initConfig(config);

    GLOBAL[this.AppNAME].Tabpanel.register(me);
  },



});












/**
 * ФОРМА ВВОДА ДАННЫХ
 * @alias Form
 * @manager ZML.FormMgr
 * @registration initComponent
 */
Ext.define('ZML.form.Form', {

  extend: 'Ext.form.Panel',
  alias: 'widget.Form',


  AppNAME: 'APP',



  listeners: {
    afterrender(obj)
    {
      console.dir(obj);
    }
  },




  initComponent(config)
  {
    var me = this;
    me.callParent(arguments);

    me.initConfig(config);

    GLOBAL[this.AppNAME].Form.register(me);
  },




});





/**
 * ТАБЛИЦА ДАННЫХ
 * @alias Grid
 * @manager ZML.GridMgr
 * @registration initComponent
 * @method GetSelectedRowField
 * @method SetSelectedRowField
 * @method GetSelectedCount
 * @method GetSelectedKeys
 * @method GetKeyName
 * @method GetSourceId
 */
Ext.define("ZML.grid.Grid", {

  extend: "Ext.grid.Panel",
  alias: "widget.Grid",

  width: '100%',
  height: '100%',
  flex: 1,
  collapsible: false,
  split: false,
  columnLines: false,
  hideHeaders: false,
  hidePaging: false,
  forceFit: true,


  MasterDataLoaded: null,

  CtxMenu: null,
  CtxMenuId: null,
  CtxMenuTitle: null,
  CtxMenuItems: [],


  selModel: {
    selType: "checkboxmodel",
    allowDeselect: true,
    mode: 'SINGLE',
  },


  listeners: {
    selectionchange: function (obj, selected, eOpts)
    {
      Ext.StoreManager.LoadLinkedStores({ scopeType: 'Grid', scopeId: obj.store.GRID.itemId });
      this.OnActiveItemChanged(obj, selected, eOpts);
    },
    cellcontextmenu: function (view, td, cellIndex, record, tr, rowIndex, e, eOpts)
    {
      e.preventDefault();
      this.OnCtxMenu();
      if (Array.isArray(this.CtxMenuItems) && this.CtxMenuItems.length)
      {
        this.CtxMenu.showAt(e.getXY());
      };
    },
    //celldblclick: function (obj, td, cellIndex, record, tr, rowIndex, e, eOpts)
    //{
    //  this.OnRowDblClicked();
    //  if (obj.editingPlugin)
    //  {
    //    obj.editingPlugin.startEditByPosition({ row: rowIndex, column: cellIndex });
    //  };
    //},
    beforecelldblclick: function (obj, td, cellIndex, record, tr, rowIndex, e, eOpts)
    {
      obj.suspendEvent('selectionchange');
    },
    aftercelldblclick: function (obj, td, cellIndex, record, tr, rowIndex, e, eOpts)
    {
      obj.resumeEvent('selectionchange');
    },

    edit: function (editor, ctx)
    {
      /** Ссылка на редактируемую запись из хранилища данных этого дерева */
      var rec = editor.context.record;
      /** Наименование поля в модели данных, в котором завершено это редактирование */
      var modelField = editor.context.field;

      /** Ссылка на столбец, в котором завершено это редактирование */
      var column = editor.context.column;
      /** Ссылка на инлайн-редактор в этом столбце */
      var coleditor = column.field;



      //ctx.grid.getStore().autoSync = false;

      if (column.COMBO_EDITOR_PLUGIN && column.COMBO_EDITOR_PLUGIN.editorValueField)
      {
        rec.set(column.COMBO_EDITOR_PLUGIN.editorValueField, coleditor.getValue());
        rec.set(column.COMBO_EDITOR_PLUGIN.cmp.dataIndex, coleditor.getDisplayValue());
        column.COMBO_EDITOR_PLUGIN.SelectValues();
      };

      rec.store.sync();

      //ctx.grid.getStore().autoSync = true;





    }

    //edit: function (editor, ctx)
    //{
    //  /** Ссылка на редактируемую запись из хранилища данных этого дерева */
    //  var rec = editor.context.record;
    //  /** Наименование поля в модели данных, в котором завершено это редактирование */
    //  var modelField = editor.context.field;

    //  /** Ссылка на столбец, в котором завершено это редактирование */
    //  var column = editor.context.column;
    //  /** Ссылка на инлайн-редактор в этом столбце */
    //  var coleditor = column.field;




    //  /** Если этот инлайн-редактор - комбобокс... */
    //  if (coleditor.is('combobox'))
    //  {
    //    /** Если только что отредактированное значение является массивом, И комбобокс - с мультивыбором... */
    //    if (Array.isArray(editor.context.value) && coleditor.multiSelect == true)
    //    {
    //      /** Объединяем отредактированное значение в единую строку через разделитель... */
    //      var val = editor.context.value.join(coleditor.delimiter);
    //      /** Обновляем данные в записи... */
    //      rec.set(modelField, val);
    //      /** Коммитим сделанные изменения */
    //      //rec.commit();
    //    }
    //    /** ...Иначе, если отредактированное значение - строка или число... */
    //    else if (typeof editor.context.value === 'string' || typeof editor.context.value === 'number')
    //    {
    //      /** Обновляем данные в записи... */
    //      rec.set(modelField, coleditor.getDisplayValue());

    //      if (column.COMBO_EDITOR_PLUGIN && column.COMBO_EDITOR_PLUGIN.editorValueField)
    //      {
    //        rec.set(column.COMBO_EDITOR_PLUGIN.editorValueField, coleditor.getValue());
    //        //editor.select(rec);
    //        column.COMBO_EDITOR_PLUGIN.SelectValues();
    //        //rec.set(column.COMBO_EDITOR_PLUGIN.editorValueField, editor.context.value);
    //        //rec.set(column.COMBO_EDITOR_PLUGIN.editorValueField, editor);
    //      };


    //      /** Коммитим сделанные изменения */
    //      //rec.commit();
    //    };
    //  }
    //  else if (Array.isArray(editor.context.value))
    //  {
    //    var val = coleditor.value.split(',');
    //    rec.set(modelField, val);
    //  };




    //  rec.store.fireEvent('datachanged', rec.store);
    //},
  },


  features: [{ ftype: 'filters' }],

  plugins: [
    {
      ptype: 'rowediting',
      clicksToEdit: 2
    },
    {
      ptype: 'GridEditor'
    },
  ],




  /** Имя приложения (Ext.application({ name: AppNAME })) */
  AppNAME: 'APP',




  initComponent: function ()
  {
    var me = this;
    Ext.getStore(me.store).GRID = me;

    this.callParent(arguments);

    me.InitCtxMenu();

    me.on('afterrender', function ()
    {
      me.InitPaging();
      if (me.hidePaging === true)
      {
        me.TogglePaging();
      };

    });

    me.on({
      viewready: me.OnViewReady,
      scope: me
    });

    GLOBAL[this.AppNAME].Grid.register(this);
  },


  columns: {
    defaults: { filterable: true },
    items: [{ dataIndex: "temptableid", header: "Загрузка..." }]
  },


  InitPaging: function ()
  {
    var t = this;
    var store = t.getStore();
    var paging = t.query('Paging')[0];
    if (paging)
    {
      t.PAGING = paging;

      paging.bindStore(store);

      //var rowsOnPage = paging.getComponent("rowsOnPage");
      var rowsOnPage = paging.PageSizeInput;

      if (!!rowsOnPage)
      {
        if (paging.defaultpagesize)
        {
          rowsOnPage.setValue(paging.defaultpagesize);
          if (store) store.pageSize = paging.defaultpagesize;
        }

        if (store) rowsOnPage.setValue(store.pageSize);

        rowsOnPage.on("blur", function (textfield)
        {
          if (store && textfield.validate())
          {
            store.pageSize = textfield.getValue();

            store.load();
            return;
          }
        });

        rowsOnPage.on("specialkey", function (textfield, e)
        {
          if (e.getKey() === e.ENTER && textfield.validate() && store)
          {
            store.pageSize = textfield.getValue();

            if (t.stateful)
            {
              var state = t.getState();
              t.fireEvent("beftatesave", t, state);
            }

            store.load();
          }
        });
      };
    }
  },


  TogglePaging: function ()
  {
    var me = this;
    var pagingtlb = me.query('paging')[0] || me.PAGING;
    if (pagingtlb)
    {
      var hidden = pagingtlb.hidden;
      pagingtlb.setVisible(hidden);
    };
  },


  InitCtxMenu: function (title, items)
  {
    var t = this;
    t.CtxMenuId = [t.itemId, 'CtxMenu'].join('-');
    if (Array.isArray(items))
    {
      t.CtxMenuItems = [].concat(items);
    };
    title = title || t.CtxMenuTitle;
    t.CtxMenu = new Ext.menu.Menu({
      id: t.CtxMenuId,
      title: title,
      Grid: t,
      animCollapse: true,
      defaults: {
        clickHideDelay: 200
      },
      items: t.CtxMenuItems
    });
  },


  AddNewRow: function ()
  {
    this.getStore().GetNewRecord();
  },


  RemoveRow: function ()
  {

  },






  /**
   * @method
   * @param {any} field
   * @param {any} recIndex
   */
  GetSelectedRowField: function (field, recIndex)
  {
    var me = this;
    recIndex = recIndex || 0;
    if (typeof me.getSelectionModel === 'function' && me.getSelectionModel())
    {
      var selmodel = me.getSelectionModel();
      var selected = selmodel.getSelection();
      var selLength = selected.length;
      if (selected && selLength)
      {
        var ret = [];

        if (selLength > 1)
        {
          ret = me.getSelectionModel().getSelection().map(function (m) { return m.get(field) });
        }
        else
        {
          if (selected[recIndex].get(field) !== undefined)
          {
            ret.push(field ? selected[recIndex].get(field) : selected[recIndex]);
          }
          else
          {
            ret.push(field ? selected[recIndex].raw[field] : selected[recIndex]);
          };
        };
        return ret;
      };
    };
  },

  /**
   * @method
   * @param {any} field
   * @param {any} val
   * @param {any} recIndex
   */
  SetSelectedRowField: function (field, val, recIndex)
  {
    var me = this;
    recIndex = recIndex || 0;
    if (typeof me.getSelectionModel === 'function' && me.getSelectionModel())
    {
      var selmodel = me.getSelectionModel();
      var selected = selmodel.getSelection();
      if (selected && selected[recIndex])
      {
        selected[recIndex].set(field, val);
      };
    };
  },

  /**
   * Возвращает количество выделенных строк в этой таблице данных.
   * @method
   */
  GetSelectedCount: function ()
  {
    return this.getSelectionModel().getSelection().length;
  },

  /**
   * Возвращает значения первичного ключа для всех выделенных строк в этой таблице.
   * @method
   * @param {Boolean} asArray Если true, то значения возвращаются в виде массива, иначе - в одну строку через запятую.
   */
  GetSelectedKeys: function (asArray)
  {
    var me = this;
    if (asArray === undefined || asArray === null)
    {
      asArray = true;
    };
    asArray = !!asArray;
    var ret = me.getSelectionModel().getSelection().map(function (m)
    {
      return m.get(me.getStore().KEY);
    });
    return asArray ? ret : ret.join(',');
  },

  /**
   * Возвращает идентификатор первичного ключа этой таблицы.
   * @method
   */
  GetKeyName: function ()
  {
    return this.getStore().KEY;
  },


  GetKeyType: function ()
  {
    var me = this;
    var keyname = me.GetKeyName();
    var keytype = me.getStore().model.getFields().find(function (f) { return f.name == keyname }).type.type;
    return keytype;
  },

  /**
   * Возвращает идентификатор хранилища данных этой таблицы.
   * @method
   */
  GetSourceId: function ()
  {
    return this.getStore().storeId;
  },








  OnActiveItemChanged: function (obj, selected, eOpts)
  {
    var t = this;
    var selmodel = t.getSelectionModel();
    var selmode = selmodel.selectionMode;
    var currselected = selmodel.getSelection();
    var selcount = currselected.length;
    var stor = t.getStore();

    switch (selcount)
    {
      case 0:
        t.OnRowDeselected();
        if (stor && typeof (stor.CheckCurrentPage) === 'function')
        {
          stor.CheckCurrentPage();
        };
        break;

      case 1: t.OnRowSelected(1); break;

      default: t.OnRowSelected(selcount); break;
    };
  },






  /**
   * Заглушка для обработчика события выделения строки в этой таблице данных.
   * @event
   */
  OnRowSelected: function () { },

  /**
   * Заглушка для обработчика события снятия выделения строки в этой таблице данных.
   * @event
   */
  OnRowDeselected: function () { },

  /**
   * Заглушка для обработчика события окончания загрузки данных в этой таблице данных.
   * @event
   */
  OnDataLoaded: function () { },

  /**
   * Заглушка для обработчика события двойного клика по строке в этой таблице данных.
   * @event
   */
  OnRowDblClicked: function () { },

  /**
   * Заглушка для обработчика события клика правой кнопкой мыши в этой таблице данных.
   * @event
   */
  OnCtxMenu: function () { },






  OnStoreLoaded: function ()
  {
    var me = this;
    var storeLinkersHaveValues = me.store.LinkersHaveValues();
    me.MasterDataLoaded = storeLinkersHaveValues;
    if (me.store && typeof (me.store.CheckCurrentPage) === 'function')
    {
      me.store.CheckCurrentPage();
    };
    if (me.PAGING && me.PAGING.CreateButton)
    {
      me.PAGING.CreateButton.setDisabled(!storeLinkersHaveValues);
    };
    me.OnDataLoaded();
  },


  OnViewReady: function (grid)
  {

    grid.keyMap = new Ext.util.KeyMap({

      target: grid.getView().getEl(),
      binding: [{
        key: 'c',
        ctrl: true,
        fn: function (keyCode, e)
        {
          console.dir('Ctrl+C pressed');
          var selcount = grid.getSelectionModel().getSelection().length;
          if (selcount > 0)
          {
            var resultText = grid.getCsvDataFromRecs();
            var ta = document.createElement('textarea');
            ta.id = 'tempcliparea';
            ta.style.position = 'absolute';
            ta.style.left = '-1000px';
            ta.style.top = '-1000px';
            ta.value = resultText;

            document.body.appendChild(ta);
            ta.focus();
            ta.select();

            document.execCommand('copy');

            setTimeout(function ()
            {
              document.body.removeChild(ta);
              console.dir(resultText);
              console.dir('Textarea removed');

            }, 500);
          };
        }
      }]
    });

  },



  getCsvDataFromRecs: function (withHeaders)
  {
    var me = this;
    var stor = me.getStore();
    var key = stor.KEY;
    var columns = me.initialConfig.columns;
    var dataIndices = columns.items.map(function (m) { return m.dataIndex; });

    var records = me.getSelectionModel().getSelection();

    var resultText = !!withHeaders ? dataIndices.join('\t') + '\n' : '';

    var res = records.map(function (rec)
    {
      var values = dataIndices.map(function (d)
      {
        var temp = rec.get(d);
        var result;
        if (typeof temp.toLocaleDateString === 'function')
        {
          result = temp.toLocaleDateString();
        }
        else if (!isNaN(parseFloat(temp)))
        {
          result = parseFloat(temp).toString().replace('.', ',');
        }
        else
        {
          result = temp;
        };
        return result;
      });
      return values.join('\t');
    });

    resultText += res.join('\n');

    return resultText;
  },



  IsVisible: function ()
  {
    var t = this;
    var result = true;
    var parent = t;
    while (parent !== undefined && !parent.is('viewport'))
    {
      parent = parent.up('component');
      if (parent && parent.is('tabpanel'))
      {
        var activetab = parent.activeTab;
        if (activetab && activetab.is('grid'))
        {
          result &= (activetab.xtype == t.xtype);
        }
        else
          if (activetab)
          {
            result &= (activetab.query('*[xtype=' + t.xtype + ']').length);
          };
        result &= parent.isVisible() && !parent.collapsed;
      }
      else
        if (parent && parent.tab && parent.tab.is('tab'))
        {
          result &= (parent.tab.active == true);
        }
        else
        {
          result &= true;
        };
    };
    return !!result;
  },


});






/**
 * СПИСОК ДАННЫХ
 * @alias List
 * @manager ZML.ListMgr
 * @registration initComponent
 * @method CountSelected
 * @method CountTotal
 * @method GetSelectedRowField
 */
Ext.define('ZML.list.List', {

  extend: 'Ext.panel.Panel',
  alias: 'widget.List',

  layout: 'fit',

  PAGING: null,
  DATAVIEW: null,
  TEMPLATE: [],
  DATA: [],
  FIELDS: [],

  config: {

    itemSelector: 'div.dataitem',
    selectedItemCls: 'selected',
    overItemCls: 'over',
    src: null,
    sel: 'single',
    hideHeader: false,
    hidePaging: false,
    iconAs: 'font-icon',
    imageSize: 60,
    pageSize: 20,
    pagingDock: 'bottom',
    innerPadding: 5,
  },


  /** Имя приложения (Ext.application({ name: AppNAME })) */
  AppNAME: 'APP',




  initComponent: function (config)
  {
    var me = this;
    me.callParent(arguments);
    me.initConfig(config);

    me.addEvents('created', 'dataviewAdded', 'pagingAdded', 'selectionChanged');
    me.on({
      created: me.OnCreated,
      dataviewAdded: me.OnDataviewAdded,
      pagingAdded: me.OnPagingAdded,
      selectionChanged: me.OnSelectionChanged,
      afterrender: me.OnAfterRender,
      scope: me
    });

    GLOBAL[this.AppNAME].List.register(me);

    me.fireEvent('created', me);
  },



  OnCreated: function (obj)
  {

  },


  OnAfterRender: function (obj)
  {
    if (obj)
    {
      obj.AddDataView();
      if (obj.getHideHeader() == true)
      {
        obj.header.hide();
      };
    };
  },


  OnDataviewAdded: function (obj)
  {
    if (obj)
    {
      obj.DATAVIEW.store.on('load', obj.OnAfterStoreLoad, obj);
      obj.AddPaging();
    };
  },


  OnPagingAdded: function (obj)
  {
    if (obj)
    {
      obj.SetTemplate(obj.TEMPLATE);
    };
  },


  OnSelectionChanged: function (obj)
  {
    if (obj)
    {
      Ext.StoreManager.LoadLinkedStores({ scopeType: 'List', scopeId: obj.itemId });
      obj.OnAfterSelectionChanged(obj);
    };
  },


  OnAfterSelectionChanged: function (obj)
  {

  },


  OnAfterStoreLoad: function (obj)
  {
    if (obj && obj.DATAVIEW)
    {
      obj.DATAVIEW.refresh();
      obj.DATAVIEW.setLoading(false);
    };
  },


  AddDataView: function ()
  {
    var me = this;
    if (!me.DATAVIEW)
    {
      me.DATAVIEW = Ext.create('Ext.view.View', {
        WRAPPER: me,
        PAGING: me.PAGING,
        itemId: me.itemId + '-InnerList',
        itemSelector: me.getItemSelector(),
        selectedItemCls: me.getSelectedItemCls(),
        overItemCls: me.getOverItemCls(),
        overflowX: 'hidden',
        overflowY: 'auto',
        width: '100%',
        height: '100%',
        border: 0,
        bodyBorder: 0,
        padding: me.innerPadding,
        flex: 1,

        plugins: {
          ptype: 'gridviewdragdrop',
          dragText: 'Drag and drop to reorganize',
          ignoreSelf: false,
          dropZone: {
            overClass: 'over-drop-zone',
            maintainOffset: true,
          },
          dragZone: {
            overClass: 'over-drop-zone'
          },
        },

        store: me.getSrc() ? Ext.getStore(me.getSrc()) : Ext.create('Ext.data.Store', {
          id: me.itemId + '-InnerStore',
          fields: me.FIELDS,
          data: me.DATA,
        }),
        listeners: {
          selectionchange: function (selmodel, selrecords)
          {
            me.fireEvent('selectionChanged', me, selmodel, selrecords);
          }
        }
      });
      if (me.getSel() === 'single' || me.getSel() === 'multi')
      {
        switch (me.getSel())
        {
          case 'single': me.DATAVIEW.getSelectionModel().setSelectionMode('SINGLE'); break;
          case 'multi': me.DATAVIEW.getSelectionModel().setSelectionMode('SIMPLE'); break;

          default: break;
        };
      };
      me.add(me.DATAVIEW);

      me.fireEvent('dataviewAdded', me);
    };
  },


  AddPaging: function ()
  {
    var me = this;
    if (!me.PAGING)
    {
      var paging = Ext.create('Ext.toolbar.Paging', {
        dock: me.getPagingDock(),
        WRAPPER: me,
        LIST: me.DATAVIEW,
        hidden: me.getHidePaging(),
        store: me.DATAVIEW.store,
      });
      paging.insert(0, { xtype: 'tbspacer', flex: 1 });
      paging.add({ xtype: 'tbspacer', flex: 1 });
      me.PAGING = paging;
      me.addDocked(paging);
      me.fireEvent('pagingAdded', me);
    };
  },


  SetTemplate: function (tplStrings)
  {
    var me = this;
    var tpl = new Ext.XTemplate(tplStrings);
    me.DATAVIEW.tpl = tpl;
    me.DATAVIEW.refresh();

    //me.DATAVIEW.REORDERER = Ext.create('Ext.ux.BoxReorderer', {
    //  animate: 100,
    //  itemSelector: 'div.data-item'
    //});
    //me.DATAVIEW.addPlugin(me.DATAVIEW.REORDERER);
    //me.DATAVIEW.REORDERER.afterFirstLayout();
  },





  /**
   * Возвращает количество выделенных элементов списка.
   * @method
   */
  CountSelected: function ()
  {
    var me = this;
    if (typeof me.DATAVIEW.getSelectionModel === 'function' && me.DATAVIEW.getSelectionModel())
    {
      var listmodel = me.DATAVIEW.getSelectionModel();
      var selected = listmodel.getSelection();
      return selected.length;
    };
  },

  /**
   * Возвращает общее количество элементов в списке.
   * @method
   */
  CountTotal: function ()
  {
    var me = this;
    if (me.DATAVIEW && me.DATAVIEW.store && me.DATAVIEW.store.isStore)
    {
      return me.DATAVIEW.store.getCount();
    }
  },

  /**
   * Возвращает значение указанного поля в модели данных 
   * для первого выделенного элемента в списке.
   * @method
   * @param {any} field
   */
  GetSelectedRowField: function (field)
  {
    var me = this;
    if (typeof me.DATAVIEW.getSelectionModel === 'function' && me.DATAVIEW.getSelectionModel())
    {
      var listmodel = me.DATAVIEW.getSelectionModel();
      var selected = listmodel.getSelection();
      var selLength = selected.length;
      if (selected && selLength)
      {
        var ret = [];
        if (selected[0].get(field) !== undefined)
        {
          ret.push(field ? selected[0].get(field) : selected[0]);
        }
        else
        {
          ret.push(field ? selected[0].raw[field] : selected[0]);
        };
        return ret;
      };
    };
  },


});















/**
 * РЕДАКТОР КОДА
 * @alias CodeMirrorPanel
 * @manager ZML.EditorMgr
 * @registration initComponent
 * @method getCodeValue @property value
 * @method setValue @property value
 * @method getMode @property mode
 * @method setMode @property mode
 * @method GetRenderedMarkdown @property htmlContent
 */
Ext.define('ZML.panel.CodeMirror', {

  extend: 'Ext.panel.Panel',
  alias: 'widget.CodeMirrorPanel',

  layout: 'fit',
  height: '100%',
  width: '100%',
  flex: 1,


  TEXTAREA: null,

  TOOLBAR: null,

  MD: null,



  config: {
    mode: null,
    tabSize: 2,
    lineNumbers: true,
    lineWrapping: true,
    //theme: 'Diff1',
    value: null,
  },



  /** Имя приложения (Ext.application({ name: AppNAME })) */
  AppNAME: 'APP',




  setMode: function (value)
  {
    var oldvalue = this.getMode();
    this.mode = value;
    this.fireEvent('modeChanged', this, oldvalue, value);
    if (oldvalue != value)
    {
      
    };
  },

  setLineNumbers: function (value)
  {
    var oldvalue = this.getLineNumbers();
    this.lineNumbers = value;
    if (oldvalue != value)
    {
      this.fireEvent('lineNumbersChanged', this, oldvalue, value);
    };
  },

  setLineWrapping: function (value)
  {
    var oldvalue = this.getLineWrapping();
    this.lineWrapping = value;
    if (oldvalue != value)
    {
      this.fireEvent('lineWrappingChanged', this, oldvalue, value);
    };
  },

  setTheme: function (value)
  {
    var oldvalue = this.getTheme();
    this.theme = value;
    if (oldvalue != value)
    {
      this.fireEvent('themeChanged', this, oldvalue, value);
    };
  },

  setValue: function (value)
  {
    var oldvalue = this.getValue() ?? this.getCodeValue();
    this.value = value;
    if (oldvalue != value)
    {
      this.fireEvent('valueChanged', this, oldvalue, value);
    };
  },




  initComponent: function (config)
  {
    var me = this;
    config = config || {};
    config.tabSize = config.tabSize || 2;
    if (config.lineNumbers == undefined) config.lineNumbers = true;
    if (config.lineWrapping == undefined) config.lineWrapping = true;
    config.theme = config.theme;


    me.addEvents('valueChanged', 'modeChanged', 'lineNumbersChanged', 'lineWrappingChanged', 'themeChanged');
    me.on({
      valueChanged: me.OnValueChanged,
      modeChanged: me.OnModeChanged,
      lineNumbersChanged: me.OnLineNumbersChanged,
      lineWrappingChanged: me.OnLineWrappingChanged,
      themeChanged: me.OnThemeChanged,
      afterrender: me.OnAfterRender,
      scope: me
    });


    me.initConfig(config);
    me.callParent(arguments);

    GLOBAL[this.AppNAME].Editor.register(me);
  },



  OnAfterRender: function ()
  {
    var me = this;
    me.TEXTAREA = Ext.create('Ext.form.field.TextArea', {
      PANEL: me,
      layout: 'fit',
      height: '100%',
      width: '100%',
      flex: 1,
      listeners: {
        afterrender: function (panel)
        {
          this.CM = CodeMirror.fromTextArea(panel.inputEl.dom, {
            content: panel.getValue(),
            mode: panel.PANEL.getMode(),
            indentWithTabs: false,
            tabSize: panel.PANEL.tabSize,
            smartIndent: true,
            lineNumbers: panel.PANEL.lineNumbers,
            autofocus: true,
            lineWrapping: panel.PANEL.lineWrapping,
            foldGutter: true,
            gutters: ['CodeMirror-linenumbers', 'CodeMirror-foldgutter'],
            matchTags: { bothTags: true },
            //specialChars: / /,
            highlightSelectionMatches:
            {
              showToken: /\w/,
              annotateScrollbar: true
            },
            theme: panel.PANEL.theme
          });
          this.CM.ParentPanel = panel;
          this.CM.on('cursorActivity', me.OnCursorActivity, me);
          this.CM.on('change', me.OnAfterChanged, me);
          panel.PANEL.setMode(panel.PANEL.mode);
          panel.PANEL.setLineNumbers(panel.PANEL.lineNumbers);
          panel.PANEL.setLineWrapping(panel.PANEL.lineWrapping);
          this.CM.setOption('tabSize', panel.PANEL.tabSize);
          this.CM.setOption('theme', panel.PANEL.theme);

          this.CM.setOption('autoCloseBrackets', true);
          this.CM.setOption('autoCloseTags', true);
          this.CM.setOption('matchBrackets', true);
        },
        change: function (widget, val)
        {
          this.CM.setValue(val);
        }
      }
    });
    me.add(me.TEXTAREA);
    me.TOOLBAR = me.ComposeToolbar();
    me.addDocked(me.TOOLBAR);


    if (me.getMode() == 'markdown' && window['markdownit'] && typeof (window['markdownit'] === 'function'))
    {
      me.MD = new window.markdownit('default', { html: true, typographer: true });
    };


    me.setMode(me.getMode());

  },

  OnValueChanged: function (obj, oldvalue, newvalue)
  {
    if (obj && obj.TEXTAREA && obj.TEXTAREA.CM)
    {
      obj.TEXTAREA.CM.setValue(newvalue);
      Ext.StoreManager.LoadLinkedStores({ scopeType: 'Editor', scopeId: obj.itemId });
    };
  },

  OnModeChanged: function (obj, oldvalue, newvalue)
  {
    if (obj && obj.TEXTAREA && obj.TEXTAREA.CM)
    {
      switch (newvalue.toLowerCase())
      {
        case 'sql': obj.TEXTAREA.CM.setOption('mode', 'text/x-sql'); obj.removeCls('JSEditor'); break;

        case 'js':
        case 'javascript': obj.TEXTAREA.CM.setOption('mode', 'text/javascript'); obj.addCls('JSEditor'); break;

        case 'json': obj.TEXTAREA.CM.setOption('mode', { name: 'text/javascript', json: true }); obj.addCls('JSEditor'); break;

        case 'cs':
        case 'csharp':
        case 'c#': obj.TEXTAREA.CM.setOption('mode', 'text/x-csharp'); obj.removeCls('JSEditor'); break;

        case 'css': obj.TEXTAREA.CM.setOption('mode', 'text/css'); obj.removeCls('JSEditor'); break;

        case 'scss': obj.TEXTAREA.CM.setOption('mode', 'text/x-scss'); obj.removeCls('JSEditor'); break;

        case 'md':
        case 'markdown': obj.TEXTAREA.CM.setOption('mode', 'markdown'); obj.removeCls('JSEditor'); break;

        case 'xml': obj.TEXTAREA.CM.setOption('mode', 'text/xml'); obj.removeCls('JSEditor'); break;


        default: break;
      };

      if (obj.TOOLBAR.ModeText)
        obj.TOOLBAR.ModeText.setText(newvalue);
    };
  },

  OnLineNumbersChanged: function (obj, oldvalue, newvalue)
  {
    if (obj && obj.TEXTAREA && obj.TEXTAREA.CM)
    {
      obj.TEXTAREA.CM.setOption('lineNumbers', newvalue);
    };
  },

  OnLineWrappingChanged: function (obj, oldvalue, newvalue)
  {
    if (obj && obj.TEXTAREA && obj.TEXTAREA.CM)
    {
      obj.TEXTAREA.CM.setOption('lineWrapping', newvalue);
    };
  },

  OnThemeChanged: function (obj, oldvalue, newvalue)
  {
    if (obj && obj.TEXTAREA && obj.TEXTAREA.CM)
    {
      obj.TEXTAREA.CM.setOption('theme', newvalue);
      obj.TOOLBAR.ThemeText.setText(newvalue);
    };
  },



  getCodeValue: function ()
  {
    if (this.TEXTAREA && this.TEXTAREA.CM)
    {
      return this.TEXTAREA.CM.getValue();
    }
    else
    {
      return null;
    };
  },


  /**
   * Очищает содержимое редактора кода.
   * @method Clear
   */
  Clear: function ()
  {
    this.setValue('');
  },


  /**
   * В случае, если режим редактирования компонента == 'markdown', 
   * и свойство MD компонента инициализировано, возвращает HTML-рендер для кода Markdown.
   * @method GetRenderedMarkdown @property htmlContent
   * @returns {String}
   */
  GetRenderedMarkdown: function ()
  {
    if (this.getMode() == 'markdown' && this.MD)
    {
      var res = this.MD.render(this.getCodeValue());
      return res;
    };
  },



  OnCursorActivity: function (c)
  {

  },


  /**
   * Обработчик события change, генерируемого дочерним элементом textarea.
   */
  OnAfterChanged: function ()
  {

  },





  ComposeToolbar: function ()
  {
    var me = this;
    var tlb = Ext.create('ZML.toolbar.CodeMirror', {
      itemId: me.itemId + '-Toolbar',
      EDITOR: me,
    });
    return tlb;
  },




  /** Свернуть все */
  FoldAll: function ()
  {
    if (globalThis.CodeMirror)
    {
      globalThis.CodeMirror.commands.foldAll(this.TEXTAREA.CM);
    };
  },

  /** Развернуть все */
  UnfoldAll: function ()
  {
    if (globalThis.CodeMirror)
    {
      globalThis.CodeMirror.commands.unfoldAll(this.TEXTAREA.CM);
    };
  },






});


/**
 * ВСПОМОГАТЕЛЬНЫЙ КЛАСС
 * ТУЛБАР ДЛЯ РЕДАКТОРА КОДА
 */
Ext.define('ZML.toolbar.CodeMirror', {

  extend: 'Ext.toolbar.Toolbar',
  alias: 'widget.EditorToolbar',


  dock: 'bottom',
  height: 36,





  EDITOR: null,



  /** Текстовый элемент, отображающий текущий режим редактирования */
  ModeText: null,

  /** Текстовый элемент, отображающий текущую тему */
  ThemeText: null,

  /** Кнопка сворачивания контента */
  FoldAllBtn: null,

  /** Кнопка разворачивания контента */
  UnfoldAllBtn: null,








  listeners: {
    afterrender: function ()
    {
      var me = this;
      me.ComposeOwnItems();
    }
  },






  initComponent: function ()
  {
    this.callParent(arguments);
  },





  ComposeOwnItems: function ()
  {
    var me = this;

    me.FoldAllBtn = Ext.create('ZML.button.Button', {
      itemId: me.itemId + '-FoldAllBtn',
      call: 'action',
      disabled: false,
      iconSet: 'mat-icons',
      __icon: 'unfold_less',
      color: 'darkgreen',
      OnHandlerCall: me.EDITOR.FoldAll.bind(me.EDITOR)
    });
    me.UnfoldAllBtn = Ext.create('ZML.button.Button', {
      itemId: me.itemId + '-UnfoldAllBtn',
      call: 'action',
      disabled: false,
      iconSet: 'mat-icons',
      __icon: 'unfold_more',
      color: 'darkgreen',
      OnHandlerCall: me.EDITOR.UnfoldAll.bind(me.EDITOR)
    });

    var items = [
      'Mode: ',
      { xtype: 'tbtext', itemId: me.itemId + '-tbMode', text: me.EDITOR.mode },
      { xtype: 'tbseparator' },
      'Theme: ',
      { xtype: 'tbtext', itemId: me.itemId + '-tbTheme', text: me.EDITOR.theme },
      { xtype: 'tbseparator' },
      me.FoldAllBtn,
      me.UnfoldAllBtn
    ];

    me.add(items);

    setTimeout(() =>
    {

      me.ModeText = me.query('#' + me.itemId + '-tbMode')[0];
      me.ThemeText = me.query('#' + me.itemId + '-tbTheme')[0];

    }, 100);
  },








});















/**
 * ЗАГРУЖАЕМЫЙ СЕРВЕРНЫЙ КОМПОНЕНТ
 * Регистрация компонента производится при вызове метода initComponent.
 * Менеджер компонента: ZML.SrvCompMgr
 * @alias ServerComponent
 */
Ext.define('ZML.ServerComponent', {

  extend: 'Ext.panel.Panel',
  alias: 'widget.ServerComponent',


  baseCls: Ext.baseCSSPrefix + 'fdl-srvcomp',

  overCls: Ext.baseCSSPrefix + 'fdl-srvcomp-over',

  width: '100%',

  height: '100%',

  flex: 1,

  layout: 'fit',

  headerPosition: 'top',


  Menu: null,


  paramLinkers: [],


  url: null,

  interval: null,

  autoLoad: false,

  callback: null,

  renderMode: 'component',


  /** Имя приложения (Ext.application({ name: AppNAME })) */
  AppNAME: 'APP',




  listeners: {
    afterrender: function ()
    {
      var tool = Ext.create('ZML.Tool', {
        icon: 'more_vert',
        callback: function (owner, tool, ev)
        {
          this.up('ServerComponent').ShowMenu(ev);
        }
      });
      this.addTool(tool);
      this.getHeader().addCls('x-fdl-srvcomp-header');
      this.setTitle(this.itemId);
    },
  },



  initComponent: function ()
  {
    this.callParent();
    this.ComposeMenu();

    GLOBAL[this.AppNAME].Component.register(this);
  },





  ComposeMenu: function ()
  {
    var me = this;
    me.Menu = new Ext.menu.Menu({
      id: this.itemId + '-Menu',
      animCollapse: true,
      defaults: { clickHideDelay: 225 },
      width: 200,
      items: [
        { text: 'Загрузить содержимое', handler: function () { me.LoadContent(); } },
        { text: 'Удалить содержимое', handler: function () { me.RemoveContent(); } }
      ]
    })
  },


  ShowMenu: function (ev)
  {
    this.Menu.showAt(ev.getXY());
    this.Menu.alignTo(this.header, 'tr-br', [-5, -5], false);
  },



  ComposeLoader: function (prms)
  {
    var me = this;
    var loader = {
      url: me.url,
      renderer: null,
      loadMask: { msg: 'Загружается...', target: me },
      callback: me.callback,
      params: prms
    };
    switch (me.renderMode)
    {
      case 'markdown': loader.renderer = me.MarkdownRenderer; break;

      default: loader.renderer = me.renderMode; break;
    };

    return loader;
  },



  MarkdownRenderer: function (loader, response, active)
  {
    if (window['markdownit'] && typeof (window['markdownit'] === 'function'))
    {
      var md = new window.markdownit('default', { html: true, typographer: true });
      var text = response.responseText;
      var result = md.render(text);
      loader.getTarget().addCls('markdown-body');
      loader.getTarget().update(result, true);
      
      return true;
    };
  },




  LoadContent: function ()
  {
    var me = this;
    var prms = {};
    me.paramLinkers.forEach(function (p)
    {
      p.compute();
      prms[p.as] = p.value;
    });
    me.removeAll();

    //var loader = {
    //  url: me.url,
    //  renderer: me.renderMode,
    //  loadMask: { msg: 'Загружается...', target: me },
    //  callback: me.callback,
    //  params: prms
    //};

    var loader = me.ComposeLoader(prms);

    var cmp = Ext.create('Ext.container.Container', {
      itemId: me.itemId + '-LoaderCont',
      loader: loader
    });

    me.add(cmp);

    cmp.getLoader().on('load', function (obj) { Prism.highlightAllUnder(obj.getTarget().el.dom) });

    cmp.getLoader().load();

    //setTimeout(() => Prism.highlightAllUnder(cmp.getLoader().getTarget().el.dom), 0);
  },


  RemoveContent: function ()
  {
    this.removeAll();
  },

});








































/** ХРАНИЛИЩЕ ДАННЫХ ДЛЯ ВЗАИМОДЕЙСТВИЯ С XML-ИСТОЧНИКОМ */
Ext.define('ZML.store.Xml', {

  extend: 'Ext.data.Store',
  alias: 'store.Xml',

  /** Адрес источника */
  url: null,

  record: null,

  idPath: null,

  root: null,



  constructor: function (config)
  {
    config = Ext.apply({
      proxy: {
        type: 'ajax',
        url: config.url,
        reader: {
          type: 'xml',
          record: config.record,
          root: config.root
        },
        writer: 'xml'
      }
    }, config);

    this.callParent([config]);
  },

});




















/** ИКОНКА */
Ext.define('ZML.Icon', {

  extend: 'Ext.Component',
  alias: 'widget.Icon',


  autoEl: {
    tag: 'span'
  },

  style: {
    borderRadius: '50%',
    cursor: 'pointer',
    transition: '120ms all ease-in-out'
  },

  baseCls: 'icon-item',
  overCls: 'icon-item-over',
  componentCls: 'mat-icons',



  config: {

    iconsize: null,

    name: null,

    color: null,
  },


  setIconsize: function (value)
  {
    var oldvalue = this.getIconsize();
    this.iconsize = value;
    if (oldvalue != value)
    {
      this.fireEvent('sizeChanged', this, oldvalue, value);
    };
  },


  setName: function (value)
  {
    var oldvalue = this.getName();
    this.name = value;
    if (oldvalue != value)
    {
      this.fireEvent('nameChanged', this, oldvalue, value);
    };
  },


  setColor: function (value)
  {
    var oldvalue = this.getColor();
    this.__lastColor = oldvalue;
    this.color = value;
    if (oldvalue != value)
    {
      this.fireEvent('colorChanged', this, oldvalue, value);
    };
  },









  initComponent: function ()
  {
    var me = this;
    me.callParent(arguments);

    me.addEvents('sizeChanged', 'nameChanged', 'colorChanged');
    me.on({
      colorChanged: me.OnColorChanged,
      sizeChanged: me.OnSizeChanged,
      nameChanged: me.OnNameChanged,
      afterrender: me.OnAfterRender,
      scope: me
    });
  },



  OnAfterRender: function (obj)
  {
    obj.fireEvent('sizeChanged', obj, null, obj.iconsize);
    obj.fireEvent('nameChanged', obj, null, obj.name);
    obj.fireEvent('colorChanged', obj, obj.__lastColor, obj.color);
    Ext.get(obj.el.dom).on('click', function (e, t, eOpts)
    {
      obj.OnClick(arguments);
    });
  },


  OnSizeChanged: function (obj, oldvalue, newvalue)
  {
    obj.setWidth(newvalue);
    obj.setHeight(newvalue);
    obj.el.dom.style.setProperty('font-size', newvalue + 'px');
  },


  OnNameChanged: function (obj, oldvalue, newvalue)
  {
    obj.el.dom.textContent = newvalue;
  },


  OnColorChanged: function (obj, oldvalue, newvalue)
  {
    obj.removeCls('color-' + oldvalue);
    obj.addCls('color-' + newvalue);
  },


  OnClick: function ()
  {

  },


});




/** ИКОНКА ИНСТРУМЕНТА */
Ext.define('ZML.Tool', {

  extend: 'Ext.Component',
  alias: 'widget.Tool',


  isTool: true,

  icon: null,

  baseCls: Ext.baseCSSPrefix + 'fdltool',

  disabledCls: Ext.baseCSSPrefix + 'fdltool-disabled',

  toolPressedCls: Ext.baseCSSPrefix + 'fdltool-pressed',

  toolOverCls: Ext.baseCSSPrefix + 'fdltool-over',

  height: 24,

  width: 24,


  renderTpl: [
    '<span id="{id}-toolEl" class="material-icons {baseCls}-{icon}">{icon}</span>'
  ],



  stopEvent: true,



  initComponent: function ()
  {
    var me = this;
    me.addEvents('click');

    Ext.applyIf(me.renderData, {
      baseCls: me.baseCls,
      icon: me.icon
    });

    me.tooltip = me.tooltip || me.qtip;
    me.callParent();
  },



  afterRender: function ()
  {
    var me = this;
    me.callParent(arguments);

    me.el.on({
      click: me.onClick,
      mousedown: me.onMouseDown,
      mouseover: me.onMouseOver,
      mouseout: me.onMouseOut,
      scope: me
    });

    if (me.tooltip)
    {
      if (Ext.quickTipsActive && Ext.isObject(me.tooltip))
      {
        Ext.tip.QuickTipManager.register(Ext.apply({ target: me.id }, me.tooltip));
      }
      else
      {
        var attr = me.tooltipType == 'qtip' ? 'data-qtip' : 'title';
        me.el.dom.setAttribute(attr, me.tooltip);
      }
    }
  },


  getFocusEl: function ()
  {
    return this.el;
  },


  setType: function (type)
  {
    var me = this;
    var oldType = me.icon;

    me.icon = type;
    if (me.rendered)
    {
      if (oldType)
      {
        me.toolEl.removeCls(me.baseCls + '-' + oldType);
      };
      me.toolEl.addCls(me.baseCls + '-' + type);
    }
    else
    {
      me.renderData.icon = type;
    }
    return me;
  },



  onClick: function (e, target)
  {
    var me = this;

    if (me.disabled)
    {
      return false;
    };

    me.el.removeCls(me.toolPressedCls);
    me.el.removeCls(me.toolOverCls);

    if (me.stopEvent !== false)
    {
      e.stopEvent();
    };

    if (me.handler)
    {
      Ext.callback(me.handler, me.scope || me, [e, target, me.ownerCt, me]);
    }
    else if (me.callback)
    {
      Ext.callback(me.callback, me.scope || me, [me.toolOwner || me.ownerCt, me, e]);
    };
    me.fireEvent('click', me, e);
    return true;
  },


  onDestroy: function ()
  {
    var me = this;

    if (Ext.quickTipsActive && Ext.isObject(me.tooltip))
    {
      Ext.tip.QuickTipManager.unregister(me.id);
    };

    if (me.keyMap)
    {
      me.keyMap.destroy();
    };
    delete me.toolOwner;
    me.callParent();
  },


  /**
   * Called when the user presses their mouse button down on a tool
   * Adds the press class ({@link #toolPressedCls})
   * @private
   */
  onMouseDown: function ()
  {
    if (this.disabled)
    {
      return false;
    };
    this.el.addCls(this.toolPressedCls);
  },


  /**
   * Called when the user rolls over a tool
   * Adds the over class ({@link #toolOverCls})
   * @private
   */
  onMouseOver: function ()
  {
    if (this.disabled)
    {
      return false;
    };
    this.el.addCls(this.toolOverCls);
  },


  /**
   * Called when the user rolls out from a tool.
   * Removes the over class ({@link #toolOverCls})
   * @private
   */
  onMouseOut: function ()
  {
    this.el.removeCls(this.toolOverCls);
    this.el.removeCls(this.toolPressedCls);
  }


});




Ext.define('ZML.CSSColor', {

  extend: 'Ext.Component',
  xtype: 'csscolor',



  mixins: {
    observable: 'Ext.util.Observable',
  },



  color: null,


  config: {

    hue: null,

    sat: null,

    light: null,

    alpha: null,
  },



  setHue: function (value)
  {
    var oldvalue = this.getHue();
    if (+value >= 0 && +value <= 360)
    {
      this.hue = value;
      if (oldvalue !== value)
      {
        this.fireEvent('changed', this);
      };
    };
  },

  setSat: function (value)
  {
    var oldvalue = this.getSat();
    if (+value >= 0 && +value <= 100)
    {
      this.sat = value;
      if (oldvalue !== value)
      {
        this.fireEvent('changed', this);
      };
    };
  },

  setLight: function (value)
  {
    var oldvalue = this.getLight();
    if (+value >= 0 && +value <= 100)
    {
      this.light = value;
      if (oldvalue !== value)
      {
        this.fireEvent('changed', this);
      };
    };
  },

  setAlpha: function (value)
  {
    var oldvalue = this.getAlpha();
    if (+value >= 0 && +value <= 1)
    {
      this.alpha = value;
      if (oldvalue !== value)
      {
        this.fireEvent('changed', this);
      };
    };
  },





  initComponent: function (config)
  {
    var me = this;
    me.initConfig(config);
    me.callParent(arguments);

    me.mixins.observable.constructor.call(me, config);
    me.addEvents('changed');

    me.on({
      changed: me.OnChanged,
      enable: me.OnEnable,
      scope: me
    });
  },



  OnChanged: function (obj)
  {
    if (obj && typeof obj.Compute === 'function' && obj.hue != null && obj.sat != null && obj.light != null && obj.alpha != null)
    {
      obj.color = obj.Compute();
      Ext.Function.defer(obj.OnAfterChanged, 0, obj, arguments);
    };
  },


  OnAfterChanged: function ()
  {

  },


  OnEnable: function ()
  {
    this.OnChanged(this);
  },



  Compute: function ()
  {
    var me = this;
    var h = me.getHue().toString();
    var s = me.getSat() + '%';
    var l = me.getLight() + '%';
    var a = me.getAlpha().toString();
    return 'hsla(' + h + ', ' + s + ', ' + l + ', ' + a + ')';
  },

});





/**
 * ПЕЙДЖИНГ-ТУЛБАР ДЛЯ ТАБЛИЦЫ ДАННЫХ
 * @alias Paging
 * 
 */
Ext.define('ZML.toolbar.Paging', {

  extend: 'Ext.toolbar.Paging',
  alias: 'widget.Paging',


  itemId: 'paging',
  dock: 'bottom',
  displayInfo: true,



  nextCls: 'logx-icon-btn mat-icons',
  nextIconCls: 'fast_forward blue',
  lastCls: 'logx-icon-btn mat-icons',
  lastIconCls: 'skip_next blue',
  prevCls: 'logx-icon-btn mat-icons',
  prevIconCls: 'fast_rewind blue',
  firstCls: 'logx-icon-btn mat-icons',
  firstIconCls: 'skip_previous blue',
  refreshCls: 'logx-icon-btn mat-icons',
  refreshIconCls: 'refresh darkblue',



  /**
   * Признак отображения кнопок "Создать"/"Изменить"/"Удалить"/"Редактировать",
   * признак поддержки соответствующих операций в родительской таблице данных.
   */
  crudButtons: false,

  /** 
   * Признак отображения кнопок "Копировать"/"Вставить",
   * признак поддержки соответствующих операций в родительской таблице данных.
   */
  clipboardButtons: false,




  /** Собственное меню тулбара */
  MENU: null,

  /** Кнопка для создания новой записи */
  CreateButton: null,

  /** Кнопка для удаления выбранных записей */
  RemoveButton: null,

  /** Кнопка для сохранения изменений в записях */
  UpdateButton: null,

  /** Кнопка для начала редактирования выделенной записи */
  EditButton: null,

  /** Кнопка для копирования выбранных записей в буфер обмена */
  CopyButton: null,

  /** Кнопка для массовой вставки новых записей из буфера обмена (НЕ ИСПОЛЬЗУЕТСЯ) */
  PasteButton: null,

  /** Кнопка для вызова меню плагина GridEditor (НЕ ИСПОЛЬЗУЕТСЯ) */
  GridEditorBtn: null,

  /** Поле ввода для изменения количества строк на странице по умолчанию */
  PageSizeInput: null,





  listeners:
  {
    afterrender: function ()
    {
      var me = this;
      var grid = me.up('grid')
      grid.PAGING = me;


      if (me.crudButtons === true)
      {
        me.CreateButton.CTX_BINDERS[0].setHostId(grid.itemId);
        me.RemoveButton.CTX_BINDERS[0].setHostId(grid.itemId);
        me.UpdateButton.CTX_BINDERS[0].setHostId(grid.itemId);
        me.EditButton.CTX_BINDERS[0].setHostId(grid.itemId);
      };

      if (me.clipboardButtons === true)
      {
        me.CopyButton.CTX_BINDERS[0].setHostId(grid.itemId);
        me.PasteButton.CTX_BINDERS[0].setHostId(grid.itemId);
      };


      me.NextButton = me.query('#next')[0];
      me.LastButton = me.query('#last')[0];
      me.PrevButton = me.query('#prev')[0];
      me.FirstButton = me.query('#first')[0];
      me.RefreshButton = me.query('#refresh')[0];


      me.NextButton.addCls(me.nextCls);
      me.NextButton.setIconCls(me.nextIconCls);
      me.LastButton.addCls(me.lastCls);
      me.LastButton.setIconCls(me.lastIconCls);
      me.PrevButton.addCls(me.prevCls);
      me.PrevButton.setIconCls(me.prevIconCls);
      me.FirstButton.addCls(me.firstCls);
      me.FirstButton.setIconCls(me.firstIconCls);
      me.RefreshButton.addCls(me.refreshCls);
      me.RefreshButton.setIconCls(me.refreshIconCls);


      me.ComposeOwnMenu();

      me.SetPageSize(me.defaultpagesize);




      // Генерируем новый itemId для поля ввода PageSizeInput,
      // регистрируем его в соответствующем менеджере компонентов.
      if (!!grid && grid.is('grid'))
      {
        me.PageSizeInput.itemId = grid.itemId + '_Paging_' + me.PageSizeInput.itemId;
        me.PageSizeInput.AppNAME = grid.AppNAME;
        GLOBAL[grid.AppNAME].Input.register(me.PageSizeInput);
      };

    },
  },



  initComponent: function ()
  {
    this.callParent(arguments);
    var me = this;
    me.ComposeOwnItems();
    me.__defaultPageSize = me.defaultpagesize
  },







  updateStatusInfo: function (text)
  {
    this.getComponent('statusInfo').setText(Ext.Date.format(new Date(), 'H:i:s') + ' ' + text);
  },


  updateSelectionInfo: function (records)
  {
    if (records > 0)
    {
      this.getComponent('selectionInfo').setText('Отмечено ' + records + ' зап.');
    }
    else
    {
      this.getComponent('selectionInfo').setText('');
    };
  },


  doRefresh: function ()
  {
    var me = this;
    var current = me.store.currentPage;
    var total = me.store.GetTotalPages();

    if (current > total)
    {
      current = 1;
      me.store.loadPage(current);
    };

    if (me.fireEvent('beforechange', me, current) !== false)
    {
      me.store.loadPage(current);
    };
  },


  ClearFilters: function ()
  {
    try
    {
      if (this.up('grid') && this.up('grid').filters)
      {
        this.up('grid').filters.clearFilters();
      };
    }
    catch (err)
    {

    };
  },




  /**
   * Создание и размещение собственных элементов управления.
   */
  ComposeOwnItems: function ()
  {
    var me = this;

    // Если crud-функционал доступен...
    if (me.crudButtons === true)
    {
      me.CreateButton = Ext.create('ZML.button.Button', {
        itemId: 'CreateNewRecord',
        call: 'action',
        disabled: true,
        iconSet: 'mat-icons',
        __icon: 'add_circle',
        color: 'darkgreen',
        needSelection: 'none',
        plugins: [
          {
            ptype: 'HostContext',
            pluginId: 'CreateNewRecord-hostContextPlugin',
            CTX_BINDERS: [
              {
                hostType: 'Grid',
                hostId: '',
                hostEvent: 'selectionchange',
                needSelection: 'none',
              }
            ]
          }
        ],
        OnHandlerCall: me.OnCreateClicked
      });
      me.RemoveButton = Ext.create('ZML.button.Button', {
        itemId: 'RemoveRecord',
        call: 'action',
        disabled: true,
        iconSet: 'mat-icons',
        __icon: 'remove_circle',
        color: 'darkred',
        needSelection: 'any',
        plugins: [
          {
            ptype: 'HostContext',
            pluginId: 'RemoveRecord-hostContextPlugin',
            CTX_BINDERS: [
              {
                hostType: 'Grid',
                hostId: '',
                hostEvent: 'selectionchange',
                needSelection: 'any',
              }
            ]
          }
        ],
        OnHandlerCall: me.OnDeleteClicked.bind(me)
      });
      me.UpdateButton = Ext.create('ZML.button.Button', {
        itemId: 'UpdateRecord',
        call: 'action',
        disabled: true,
        iconSet: 'mat-icons',
        __icon: 'save',
        color: 'darkgreen',
        needSelection: 'any',
        plugins: [
          {
            ptype: 'HostContext',
            pluginId: 'UpdateRecord-hostContextPlugin',
            CTX_BINDERS: [
              {
                hostType: 'Grid',
                hostId: '',
                hostEvent: 'selectionchange',
                needSelection: 'any',
              }
            ]
          }
        ],
        OnHandlerCall: me.OnUpdateClicked.bind(me)
      });
      me.EditButton = Ext.create('ZML.button.Button', {
        itemId: 'EditRecord',
        call: 'action',
        disabled: true,
        iconSet: 'mat-icons',
        __icon: 'edit',
        color: 'darkgreen',
        needSelection: 'single',
        plugins: [
          {
            ptype: 'HostContext',
            pluginId: 'EditRecord-hostContextPlugin',
            CTX_BINDERS: [
              {
                hostType: 'Grid',
                hostId: '',
                hostEvent: 'selectionchange',
                needSelection: 'single',
              }
            ]
          }
        ],
        OnHandlerCall: me.OnEditClicked.bind(me)
      });

      me.insert(0, me.CreateButton);
      me.insert(1, me.RemoveButton);
      me.insert(2, me.UpdateButton);
      me.insert(3, me.EditButton);
      me.insert(4, { xtype: 'tbseparator', margin: '0 3' });
    };

    // Если доступен функционал взаимодействия с буфером обмена...
    if (me.clipboardButtons === true)
    {
      me.CopyButton = Ext.create('ZML.button.Button', {
        itemId: 'CopyRecords',
        call: 'action',
        disabled: true,
        iconSet: 'mat-icons',
        __icon: 'post_add',
        color: 'brown',
        tooltip: 'Скопировать выбранные записи в буфер обмена',
        needSelection: 'any',
        plugins: [
          {
            ptype: 'HostContext',
            pluginId: 'CopyRecords-hostContextPlugin',
            CTX_BINDERS: [
              {
                hostType: 'Grid',
                hostId: '',
                hostEvent: 'selectionchange',
                needSelection: 'any',
              }
            ]
          }
        ],
        OnHandlerCall: me.OnCopyClicked
      });
      me.PasteButton = Ext.create('ZML.button.Button', {
        itemId: 'PasteRecords',
        call: 'action',
        iconSet: 'mat-icons',
        __icon: 'archive',
        color: 'brown',
        tooltip: 'Вставить новые записи из буфера обмена',
        needSelection: 'none',
        plugins: [
          {
            ptype: 'HostContext',
            pluginId: 'PasteRecords-hostContextPlugin',
            CTX_BINDERS: [
              {
                hostType: 'Grid',
                hostId: '',
                hostEvent: 'selectionchange',
                needSelection: 'none',
              }
            ]
          }
        ],
        OnHandlerCall: me.OnPasteClicked
      });
    };

    // Создаем кнопку вызова меню...
    me.GridEditorBtn = Ext.create('ZML.button.Button', {
      itemId: 'GridEditorMenuBtn',
      call: 'action',
      iconSet: 'mat-icons',
      __icon: 'more_vert',
      color: 'grey',
      tooltip: '',
      needSelection: 'none',
      OnHandlerCall: function (item, evt)
      {
        me.ShowOwnMenu(item, evt);
      }
    });
    // Создаем поле ввода pageSize
    me.PageSizeInput = Ext.create('Ext.form.field.Number', {
      itemId: 'rowsOnPage',
      labelWidth: 45,
      fieldLabel: 'На стр.:',
      width: 110,
      tooltip: 'Количество строк на странице',
      minValue: 1,
      maxValue: 9999,
      //listeners: {
      //  change: function (it, newval, oldval) {
      //    me.SetPageSize(newval);
      //  }
      //}
    });


    me.insert(me.crudButtons ? 5 : 0, me.GridEditorBtn);
    me.add({ xtype: 'tbseparator', margin: '0 3' });
    me.add(me.PageSizeInput);
  },


  /**
   * Создание меню.
   */
  ComposeOwnMenu: function ()
  {
    var me = this;
    var grid = me.up('grid');

    var showAllItem = {
      xtype: 'menucheckitem',
      itemId: 'Item1',
      text: 'Отображать все записи',
      checked: false,
      handler: function (it)
      {
        var host = it.parentMenu.HOST;
        if (it.checked === true)
        {
          host.SetPageSize(host.up('grid').getStore().totalCount);
        }
        else
        {
          host.SetPageSize();
        };
      }
    };

    var menuItems = me.clipboardButtons === true ? [showAllItem, me.CopyButton, me.PasteButton] : [showAllItem];

    me.MENU = new Ext.menu.Menu({

      id: grid.itemId + '-Paging-InnerMenu',
      animCollapse: true,
      allowOtherMenus: true,
      defaults: { clickHideDelay: 200 },
      HOST: me,
      items: menuItems
    });
  },

  ShowOwnMenu: function (it, ev)
  {
    this.MENU.showAt(ev.getXY());
    this.MENU.alignTo(it, 'bl?', [0, -90], false);
  },






  /**
   * Устанавливает свойство pageSize в связанном хранилище данных,
   * равным значению в поле ввода PageSizeInput, после чего инициирует загрузку данных в этом хранилище.
   */
  SyncPageSize: function ()
  {
    var me = this;
    var amount = me.PageSizeInput.getValue();

    if (amount !== undefined && amount !== null)
    {
      me.__defaultPageSize = +amount;

      me.up('grid').getStore().pageSize = +amount;
      me.defaultpagesize = +amount;
      me.doRefresh();
    };
  },

  /**
   * Устанавливает свойство pageSize в связанном хранилище данных,
   * равным указанному значению, после чего инициирует загрузку данных в этом хранилище.
   * @param {any} amount количество элементов на странице.
   */
  SetPageSize: function (amount)
  {
    var me = this;

    if (amount !== undefined && amount !== null)
    {
      if (+amount <= me.PageSizeInput.maxValue && +amount >= me.PageSizeInput.minValue)
      {
        me.__defaultPageSize = me.PageSizeInput.getValue();

        me.up('grid').getStore().pageSize = +amount;
        me.PageSizeInput.setValue(+amount);
        me.defaultpagesize = +amount;
        me.doRefresh();
      };
    }
    else
    {
      me.up('grid').getStore().pageSize = me.__defaultPageSize;
      me.PageSizeInput.setValue(me.__defaultPageSize);
      me.defaultpagesize = me.__defaultPageSize;
      me.doRefresh();
    };
  },





  /** Обработчик нажатия на кнопку "Создать" */
  OnCreateClicked: function ()
  {
    var grid = this.up('grid');
    if (grid && grid.GRID_EDITOR && this.crudButtons === true)
    {
      grid.GRID_EDITOR.PrepareNewRows();
    };
  },

  /** Обработчик нажатия на кнопку "Удалить" */
  OnDeleteClicked: function ()
  {
    var grid = this.up('grid');
    if (grid && typeof (grid.getStore) === 'function' && this.crudButtons === true)
    {
      var records = [].concat(grid.getSelectionModel().getSelection());
      grid.getStore().remove(records);
    };
  },

  /** Обработчик нажатия на кнопку "Сохранить изменения" */
  OnUpdateClicked: function ()
  {
    var grid = this.up('grid');
    if (grid && typeof (grid.getStore) === 'function' && this.crudButtons === true)
    {
      grid.getStore().sync();
    };
  },

  /** Обработчик нажатия на кнопку "Начать редактирование" */
  OnEditClicked: function ()
  {
    var grid = this.up('grid');
    if (grid && grid.editingPlugin && this.crudButtons === true)
    {
      grid.editingPlugin.startEdit(grid.GetSelectedRowField()[0], 0);
    };
  },

  /** Обработчик нажатия на кнопку "Копировать в буфер обмена" */
  OnCopyClicked: function ()
  {
    var grid = this.up('grid');
    if (grid && grid.GRID_EDITOR && this.clipboardButtons === true)
    {
      grid.GRID_EDITOR.ExportToClipboard();
    };
  },

  /** Обработчик нажатия на кнопку "Вставить из буфера обмена" */
  OnPasteClicked: function ()
  {

  },





  __defaultPageSize: null,

});






Ext.define('ZML.column.Number', {

  extend: 'Ext.grid.column.Number',
  alias: 'widget.numbercol',
  format: '0',
  width: 50,
  align: 'center',


  initComponent: function ()
  {
    var t = this;
    if (!this.renderer)
    {
      this.renderer = function (value)
      {
        if (!value && !t.format)
        {
          return '';
        };
        if (value !== undefined && value !== null)
        {
          return Ext.util.Format.number(value, t.format);
        };
      };
    };
    this.callParent(arguments);
  }
});




Ext.define('ZML.column.Date', {

  extend: 'Ext.grid.column.Date',
  alias: 'widget.datecol',

  format: 'd.m.Y H:i:s',
  emptyCellText: '',
  align: 'center',


  initComponent: function ()
  {
    var t = this;
    this.renderer = function (value)
    {
      return (value instanceof Date) ? Ext.util.Format.date(value, t.format) : t.emptyCellText;
    };
    this.callParent(arguments);
  }
});











Ext.define('ZML.menu.MonthMenu', {

  extend: 'Ext.menu.Menu',

  idField: 'id',
  labelField: 'name',
  loadOnShow: true,
  single: false,
  plain: true,

  options: [
    { id: '01', name: 'январь' },
    { id: '02', name: 'февраль' },
    { id: '03', name: 'март' },
    { id: '04', name: 'апрель' },
    { id: '05', name: 'май' },
    { id: '06', name: 'июнь' },
    { id: '07', name: 'июль' },
    { id: '08', name: 'август' },
    { id: '09', name: 'сентябрь' },
    { id: '10', name: 'октябрь' },
    { id: '11', name: 'ноябрь' },
    { id: '12', name: 'декабрь' },
  ],


  constructor: function (cfg)
  {
    var me = this;
    me.selected = [];
    me.addEvents('checkchange');

    me.callParent([cfg = cfg || {}]);

    me.store = Ext.create('Ext.data.ArrayStore', {
      fields: [me.idField, me.labelField],
      data: [].concat(me.options),
      listeners: {
        load: me.onLoad,
        scope: me
      }
    });
    me.loaded = true;
    me.autoStore = true;
  },


  destroy: function ()
  {
    this.store.destroyStore();
    this.callParent();
  },


  show: function ()
  {
    var me = this;
    me.callParent();
    if (me.loadOnShow && !me.loaded && !me.store.loading)
    {
      me.store.load();
    }
  },


  onLoad: function (store, records)
  {
    var me = this;
    var listeners = {
      checkchange: me.checkChange,
      scope: me
    };

    Ext.suspendLayouts();
    me.removeAll(true);
    gid = me.single ? Ext.id() : null;
    me.options.forEach(function (opt)
    {
      me.add(Ext.create('Ext.menu.CheckItem', {
        text: opt[me.labelField],
        group: gid,
        checked: Ext.Array.contains(me.selected, opt[me.idField]),
        hideOnClick: true,
        clickHideDelay: 250,
        value: opt[me.idField],
        listeners: listeners
      }));
    });

    me.loaded = true;
    Ext.resumeLayouts(true);
    me.fireEvent('load', me, records);

  },


  getSelected: function ()
  {
    return this.selected;
  },


  setSelected: function (value)
  {
    value = this.selected = [].concat(value);

    if (this.loaded)
    {
      this.items.each(function (item)
      {
        item.setChecked(false, true);
        for (var i = 0, len = value.length; i < len; i++)
        {
          if (item.value == value[i])
          {
            item.setChecked(true, true);
          }
        }
      });
    }
  },


  checkChange: function (item, checked)
  {
    var value = [];
    this.items.each(function (item)
    {
      if (item.checked)
      {
        value.push(item.value);
      }
    });
    this.selected = value;

    this.fireEvent('checkchange', item, checked);
  },

});



Ext.define('ZML.filter.MonthFilter', {

  extend: 'Ext.ux.grid.filter.Filter',
  alias: 'gridfilter.month',


  phpMode: true,


  init: function (config)
  {
    this.dt = Ext.create('Ext.util.DelayedTask', this.fireUpdate, this);
  },


  createMenu: function (config)
  {
    var menu = Ext.create('ZML.menu.MonthMenu', config);
    menu.on('checkchange', this.onCheckChange, this);
    return menu;
  },


  getValue: function ()
  {
    return this.menu.getSelected();
  },


  setValue: function (value)
  {
    this.menu.setSelected(value);
    this.fireEvent('update', this);
  },


  isActivatable: function ()
  {
    return this.getValue().length > 0;
  },


  getSerialArgs: function ()
  {
    return { type: 'month', value: this.phpMode ? this.getValue().join(',') : this.getValue() };
  },


  onCheckChange: function ()
  {
    this.dt.delay(this.updateBuffer);
  },


  validateRecord: function (record)
  {
    var valuesArray = this.getValue();
    return Ext.Array.indexOf(valuesArray, record.get(this.dataIndex)) > -1;
  },


});










Ext.data.proxy.Rest.override({


  lastResponseStatusCode: null,

  lastResponseHeaders: {},


  afterRequest: function (request, success)
  {
    if (request && request.operation && request.operation.response)
    {
      this.lastResponseStatusCode = request.operation.response.status;
      this.lastResponseHeaders = Ext.apply({}, request.operation.response.getAllResponseHeaders());
    };
  }

});









Ext.define('ZML.data.RESTProxy', {

  extend: 'Ext.data.proxy.Rest',
  alias: 'proxy.RESTProxy',


  url: '',
  path: '',
  timeout: 600000,
  appendId: true,


  actionMethods: {
    create: 'POST',
    read: 'GET',
    update: 'PUT',
    destroy: 'DELETE'
  },

  reader: {
    type: 'json',
    root: 'data',
    //useSimpleAccessors: true,
    idProperty: 'Id',
    totalProperty: 'totalCount',
    successProperty: 'success',
    messageProperty: 'errorText',
    listeners: {
      exception: function (reader, response, error, eOpts)
      {
        console.error('Exception in JSON Reader:', reader, response, Ext.decode(response.responseText))
      }
    }
  },

  writer: {
    type: 'json',
    successProperty: 'success',
    messageProperty: 'errorText',
    writeAllFields: true,
    listeners: {
      exception: function (reader, response, error, eOpts) { }
    }
  },


  constructor: function ()
  {
    this.callParent(arguments)
    this.url = this.url + this.path
    this.noCache = false
  }
});







Ext.define('ZML.data.RestStore', {

  extend: 'Ext.data.Store',
  alias: 'store.RestStore',

  currentPage: 1,
  autoSync: true,

  ObjectType: null,

  paramLinkers: [],

  listeners:
  {
    beforeload: function ()
    {
      this.init()
      this.initParamLinkers()
      this.processParamLinkers()
    },
    load: function (st)
    {
      var me = this
      this.__paramLinkers = []
      if (me.GRID)
      {
        me.GRID.OnStoreLoaded()
      };
      if (typeof me.OnAfterLoad === 'function')
      {
        Ext.Function.defer(me.OnAfterLoad, 0, st, [me.__Caller])
      };
    },
    refresh: function (st)
    {
      if (st && st.isStore && st.GRID)
      {
        st.GRID.OnStoreLoaded()
      };
    },
    write: function (st)
    {
      var me = this
      if (me.__appendId != null && me.__appendId != undefined)
      {
        me.proxy.appendId = me.__appendId
      };


      setTimeout(function ()
      {

        //me.load()
        if (typeof me.OnAfterWrite === 'function')
        {
          Ext.Function.defer(me.OnAfterWrite, 500, st, [me.__Caller])
        };

      }, 500);
    },
  },


  proxy:
  {
    type: 'RESTProxy',
    extraParams: {},
  },

  init: function ()
  {
    var t = this
    t.getProxy().extraParams = {}
  },

  initParamLinkers: function ()
  {
    var t = this
    t.__paramLinkers = [].concat(t.paramLinkers)
  },

  processParamLinkers: function ()
  {
    var t = this
    var proxy = t.getProxy()
    t.__paramLinkers.forEach(function (p)
    {
      p.compute()
      proxy.extraParams[p.as] = p.value
    })
  },


  GetRecord: function (recIndex)
  {
    var me = this
    recIndex = recIndex || 0
    var ret = me.getAt(recIndex)
    return ret
  },


  GetRecordField: function (fieldName, recIndex)
  {
    var me = this
    recIndex = recIndex || 0
    if (fieldName)
    {
      if (typeof me.getAt === 'function' && me.getAt(recIndex))
      {
        return me.getAt(recIndex).get(fieldName)
      };
    }
    else
    {
      return me.GetRecord(recIndex)
    };
  },


  SetRecordField: function (fieldName, val, recIndex)
  {
    var me = this
    recIndex = recIndex || 0
    if (fieldName)
    {
      me.GetRecord(recIndex).set(fieldName, val)
    };
  },


  GetTotalPages: function ()
  {
    var me = this
    return me.pageSize > 0 ? Math.ceil(me.totalCount / me.pageSize) : 0
  },



  CheckCurrentPage: function ()
  {
    if (this.currentPage > this.GetTotalPages() && this.GetTotalPages() > 0)
    {
      this.currentPage = 1;
      this.load();
    };
  },








  GetNewRecord: function ()
  {
    var me = this
    var modelName = me.model.getName()
    var model = Ext.create(modelName)
    me.add(model)
  },


  LinkersHaveValues: function ()
  {
    var me = this
    var countLinkers = me.paramLinkers.length
    return countLinkers == 0 || me.paramLinkers.every(function (ev)
    {
      return ev.value !== undefined && ev.value !== null && ev.value !== -1
    })
  },


  AddAndSaveRecord: function (rec)
  {
    var me = this
    me.__appendId = me.proxy.appendId
    me.proxy.extraParams = {}
    me.proxy.appendId = false
    rec.phantom = rec.phantom || true
    me.add(rec)

    if (me.getProxy().type != "memory")
    {
      me.last().phantom = true
      me.last().setDirty();
      me.sync();
      //me.save()
    };

    setTimeout(function ()
    {
      me.proxy.appendId = me.__appendId;

      //me.load()
      if (typeof me.OnAfterWrite === 'function')
      {
        Ext.Function.defer(me.OnAfterWrite, 500, me, [me.__Caller])
      };

    }, 500);
  },



  __paramLinkers: [],


  OnAfterLoad: null,

  OnAfterWrite: null,

  __Caller: null,

  __appendId: null,

});


















Ext.define('ZML.panel.d3.ClusterDendrogram', {

  extend: 'Ext.panel.Panel',
  alias: 'widget.D3Cluster',


  bodyPadding: 10,
  overflowX: 'auto',
  overflowY: 'auto',
  width: '100%',
  height: '100%',



  config: {

    jsondata: null,
  },



  strokeColor: '#999',

  strokeOpacity: 0.4,

  strokeWidth: 1,

  colorOver: 'hsl(195, 60%, 45%)',

  itemdX: 13,

  circle: {
    radius: 2.5,
    nodeColor: '#555',
    leafColor: '#AAA'
  },





  setJsondata: function (value)
  {
    var oldvalue = this.getJsondata();
    this.jsondata = value;
    if (oldvalue != value)
    {
      this.fireEvent('jsondataChanged', this, oldvalue, value);
    };
  },





  initComponent: function (config)
  {
    var me = this;


    me.addEvents('jsondataChanged');
    me.on({
      jsondataChanged: me.OnJsondataChanged,

      scope: me
    });


    me.initConfig(config);
    me.callParent(arguments);
  },




  OnJsondataChanged: function (obj, oldval, newval)
  {
    if (obj && newval)
    {
      obj.body.el.dom.querySelector('span > div').innerHTML = '';
      var item = obj.ComposeSvgRoot();
      obj.body.el.dom.querySelector('span > div').append(item);
    };
  },







  ComposeSvgRoot: function ()
  {
    var me = this;


    function autoBox()
    {
      document.body.appendChild(this);
      const { x, y, width, height } = this.getBBox();
      document.body.removeChild(this);
      return [x, y, width, height];
    };


    //function overed(event, d)
    //{
    //  d3.select(this).attr('fill', me.colorOver);
    //  d3.select(d3.select(this).select(d => d.parent).node()).attr('fill', me.colorOver);
    //  //d3.selectAll(d.map(d => d.source.text)).attr('fill', me.colorOver);
    //};
    //function outed(event, d)
    //{
    //  d3.select(this).attr('fill', null);
    //  d3.select(d3.select(this).select(d => d.parent).node()).attr('fill', null);
    //  //d3.selectAll(d.map(d => d.target.text)).attr('fill', null);
    //  //d3.selectAll(d.map(d => d.source.text)).attr('fill', null);
    //};




    var tree = data =>
    {
      const root = d3.hierarchy(data).sort((a, b) => d3.descending(a.height, b.height) || d3.ascending(a.data.name, b.data.name));
      root.dx = me.itemdX;
      root.dy = me.getWidth() / (root.height + 1);
      return d3.cluster().nodeSize([root.dx, root.dy])(root);
    };




    const root = tree(me.jsondata);

    const svg = d3.create("svg");

    svg.append("g")
      .attr("fill", "none")
      .attr("stroke", me.strokeColor)
      .attr("stroke-opacity", me.strokeOpacity)
      .attr("stroke-width", me.strokeWidth)
      .selectAll("path")
      .data(root.links())
      .join("path")
      .attr("d", d => `
        M${d.target.y},${d.target.x}
        C${d.source.y + root.dy / 2},${d.target.x}
         ${d.source.y + root.dy / 2},${d.source.x}
         ${d.source.y},${d.source.x}
      `)
      ;

    svg.append("g")
      .selectAll("circle")
      .data(root.descendants())
      .join("circle")
      .attr("cx", d => d.y)
      .attr("cy", d => d.x)
      .attr("fill", d => d.children ? me.circle.nodeColor : me.circle.leafColor)
      .attr("r", me.circle.radius);

    svg.append("g")
      .attr("font-family", "sans-serif")
      .attr("font-size", 10)
      .attr("stroke-linejoin", "round")
      .attr("stroke-width", 3)
      .selectAll("text")
      .data(root.descendants())
      .join("text")
      .attr("x", d => d.y)
      .attr("y", d => d.x)
      .attr("dy", "0.31em")
      .attr("dx", d => d.children ? -6 : 6)
      .text(d => d.data.name)
      //.on('mouseover', overed)
      //.on('mouseout', outed)
      .filter(d => d.children)
      .attr("text-anchor", "end")
      .clone(true).lower()
      .attr("stroke", "white")
      ;

    return svg.attr("viewBox", autoBox).node();
  },






});








Ext.define('Zml.tree.Tree', {

  extend: 'Ext.tree.Panel',
  alias: 'widget.Tree',
  xtype: 'Tree',

  width: '100%',
  height: '100%',
  collapsible: false,
  titleCollapse: false,
  split: false,

  cls: '',
  rowCls: '',
  fieldsForRowCls: [],
  ExtraToolbars: [],

  useArrows: true,
  rootVisible: false,
  allowDeselect: true,
  expanded: false,
  dataLoaded: false,

  plugins: [
    {
      ptype: 'cellediting',
      clicksToEdit: 2,
      pluginId: 'cedit'
    }
  ],

  viewConfig: {
    plugins: [
      {
        ptype: 'treeviewdragdrop',
        dragText: 'Drag and drop to reorganize',
        allowCopy: true,
        allowContainerDrops: true,
        allowParentInserts: true,
        containerScroll: true,
        expandDelay: 300,
        sortOnDrop: true,
        nodeHighlightColor: '74aaf1',
        pluginId: 'commonTreeDD'
      }
    ]
  },


  listeners: {


    afterrender: function ()
    {
      var me = this;
      var tlb = me.ComposeToolbar('top');
      var extraTlbs = me.GetExtraToolbars('top');
      me.addDocked(tlb);
      me.addDocked(extraTlbs);

      if (me.store && me.store.isStore)
      {
        me.store.TREE = me;
        me.relayEvents(me.store, ['datachanged', 'update']);

        me.on({
          datachanged: me.OnStoreDatachanged,
          scope: me
        });
      };
    },


    edit: function (editor, ctx)
    {
      /** Ссылка на редактируемую запись из хранилища данных этого дерева */
      var rec = editor.context.record;
      /** Наименование поля в модели данных, в котором завершено это редактирование */
      var modelField = editor.context.field;

      /** Ссылка на столбец, в котором завершено это редактирование */
      var column = editor.context.column;
      /** Ссылка на инлайн-редактор в этом столбце */
      var coleditor = column.field;


      /** Если этот инлайн-редактор - комбобокс... */
      if (coleditor.is('combobox'))
      {
        /** Если только что отредактированное значение является массивом, И комбобокс - с мультивыбором... */
        if (Array.isArray(editor.context.value) && coleditor.multiSelect == true)
        {
          /** Объединяем отредактированное значение в единую строку через разделитель... */
          var val = editor.context.value.join(coleditor.delimiter);
          /** Обновляем данные в записи... */
          rec.set(modelField, val);
          /** Коммитим сделанные изменения */
          //rec.commit();
        }
        /** ...Иначе, если отредактированное значение - строка или число... */
        else if (typeof editor.context.value === 'string' || typeof editor.context.value === 'number')
        {
          /** Обновляем данные в записи... */
          rec.set(modelField, editor.context.value);
          /** Коммитим сделанные изменения */
          //rec.commit();
        };
      };


      rec.store.fireEvent('datachanged', rec.store);
    },


    selectionchange: function (obj, selected, eOpts)
    {
      Ext.StoreManager.LoadLinkedStores({ scopeType: 'tree', scopeId: obj.view.panel.itemId });
    },
  },




  initComponent: function ()
  {
    var me = this;
    me.callParent(arguments);
  },


  AddRowClassFromField: function (field)
  {
    var me = this;
    Array.from(me.view.getNodes()).forEach(function (node)
    {
      var rec = me.view.getRecord(node);
      var clsFirst = (me.rowCls || '');
      var clsSecond = (rec.get(field) || '');
      var cls = clsFirst + (!!clsFirst && !!clsSecond ? '-' : '') + clsSecond;
      node.classList.add(cls);
    });
  },


  GetSelectedRowField: function (field)
  {
    var me = this;
    if (typeof me.getSelectionModel === 'function' && me.getSelectionModel())
    {
      var selmodel = me.getSelectionModel();
      var selected = selmodel.getSelection();
      var selLength = selected.length;
      if (selected && selLength)
      {
        var ret = [];
        if (selected[0].get(field) !== undefined)
        {
          ret.push(field ? selected[0].get(field) : selected[0]);
        }
        else
        {
          ret.push(field ? selected[0].raw[field] : selected[0]);
        };
        return ret;
      };
    };
  },


  ComposeToolbar: function (dock)
  {
    var me = this;
    dock = dock || 'top';

    var sep = { xtype: 'tbseparator' };

    var btnStoreLoad = Ext.create('Zml.button.Button', {
      itemId: 'btnStoreLoad',
      iconSet: 'mat-icons',
      __icon: 'get_app',
      color: 'darkgreen',
      tooltip: 'Загрузить/обновить данные',
      condition: function () { return true; },
      OnHandlerCall: function ()
      {
        me.store.load();
      },
    });

    var btnCollapseAll = Ext.create('Zml.button.Button', {
      itemId: 'btnCollapseAll',
      iconSet: 'mat-icons',
      __icon: 'unfold_less',
      color: 'blue',
      tooltip: 'Свернуть все',
      condition: function () { return true; },
      OnHandlerCall: function ()
      {
        me.collapseAll();
      },
    });

    var btnExpandAll = Ext.create('Zml.button.Button', {
      itemId: 'btnExpandAll',
      iconSet: 'mat-icons',
      __icon: 'unfold_more',
      color: 'blue',
      tooltip: 'Развернуть все',
      disabled: true,
      condition: function () { return true; },
      OnHandlerCall: function ()
      {
        me.expandAll();
      },
    });

    var btnUpdateAll = Ext.create('Zml.button.Button', {
      itemId: 'btnUpdateAll',
      iconSet: 'mat-icons',
      __icon: 'save',
      color: 'darkgreen',
      tooltip: 'Сохранить все изменения',
      disabled: true,
      condition: function () { return this.up('treepanel').getStore().getUpdatedRecords().length > 0 },
      OnHandlerCall: function ()
      {
        me.store.UpdateRecords();
      },
    });


    var tlb = Ext.create('Ext.toolbar.Toolbar', {
      dock: dock,
      items: [
        btnStoreLoad, sep, btnCollapseAll, btnExpandAll, sep, btnUpdateAll
      ]
    });

    me.ExtraToolbars.filter(function (f) { return f.dock == dock }).forEach(function (btn)
    {
      tlb.add(btn);
    });

    return tlb;
  },


  GetExtraToolbars: function (allExcept)
  {
    return this.ExtraToolbars.filter(function (f) { return f.dock != allExcept });
  },




  OnStoreLoaded: function ()
  {
    var me = this;
    var storeLinkersHaveValues = me.store.LinkersHaveValues();


    //if (me.store.proxy.reader.jsonData && me.store.proxy.reader.jsonData.data)
    //{
    //  me.getRootNode().set('Id', me.store.proxy.reader.jsonData.data[0].Id);
    //  me.getRootNode().set('Name', me.store.proxy.reader.jsonData.data[0].Name);
    //};




    //if (me.dataLoaded == true)
    //{
    //  if (me.store.proxy.reader.jsonData && me.store.proxy.reader.jsonData.data)
    //  {
    //    me.getRootNode().appendChild(me.store.proxy.reader.jsonData.data[0].children);
    //  };
    //}
    //else
    //{
    //  me.getRootNode().appendChild(me.store.proxy.reader.jsonData.data[0].children);
    //  me.dataLoaded = true;
    //};    
    //if (me.expanded == true)
    //{
    //  me.expandAll();
    //}
    //else
    //{
    //  me.collapseAll();
    //};

    //setTimeout(function ()
    //{
    //  me.fieldsForRowCls.forEach(function (fld)
    //  {
    //    me.AddRowClassFromField(fld);
    //  });
    //}, 100);
  },


  OnStoreDatachanged: function ()
  {
    var updatedCount = this.getStore().getUpdatedRecords().length;
    var addedCount = this.getStore().getNewRecords().length;
    var removedCount = this.getStore().getRemovedRecords().length;

    this.ManageButtonsState();
  },





  /**
   * Возвращает узел дерева по заданному идентификатору узла.
   * @param {String | Number} id Идентификатор разворачиваемого узла.
   */
  FindNodeById: function (id)
  {
    return this.store.getNodeById(id);
  },

  /**
   * Разворачивает узел дерева с заданным идентификатором.
   * @param {String | Number} id Идентификатор разворачиваемого узла.
   * @param {Boolean} closestChildren Если равно true, то также разворачивает все потомки 1-го уровня этого узла.
   */
  ExpandNodeById: function (id, closestChildren)
  {
    var me = this;
    var node = this.FindNodeById(id);

    if (node && node.get('leaf') == false && node.get('expanded') == false)
    {
      me.GetNodeAncestors(id).forEach(function (a)
      {
        me.expandNode(a, false, function () { a.isExpandingOrCollapsing = false; })
      });


      setTimeout(function ()
      {

        me.expandNode(node, false, function ()
        {
          node.isExpandingOrCollapsing = false;
          if (closestChildren === true)
          {
            node.childNodes.filter(function (f) { return f.get('leaf') == false && f.get('expanded') == false }).forEach(function (n)
            {
              me.expandNode(n, false, function () { n.isExpandingOrCollapsing = false; });
            });
          };
        }, me);

      }, 10);

    };
  },

  /**
   * Возвращает список узлов-предков для узла с заданным идентификатором.
   * @param {any} id Идентификатор узла, для которого выполняется поиск предков.
   */
  GetNodeAncestors: function (id)
  {
    var me = this;
    var ret = [];
    var startfrom = me.FindNodeById(id);

    if (!startfrom || startfrom.get('root') == true)
    {
      return ret;
    }
    else
    {
      var parent = startfrom.parentNode;
      while (parent && parent.get('root') == false)
      {
        ret.push(parent);
        parent = parent.parentNode;
      };
      ret.push(me.getRootNode());
    };
    return ret.reverse();
  },







  ManageButtonsState: function ()
  {
    var me = this;
    var buttons = me.query('button');
    //var selcount = me.CountSelected();

    buttons.forEach(function (btn)
    {
      var needsel = btn.needSelection;
      //var btnid = btn.itemId;
      var condition = btn.condition();


      switch (needsel)
      {
        case 'none': btn.setDisabled(false && !condition); break;
        case 'single': btn.setDisabled(selcount != 1 || !condition); break;
        case 'multi': btn.setDisabled(selcount < 2 && !condition); break;
        case 'any': btn.setDisabled(selcount == 0 && !condition); break;

        default: btn.setDisabled(!condition); break;
      };
    });



  },


});